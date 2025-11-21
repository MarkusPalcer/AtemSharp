using System.Diagnostics;
using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

/// <summary>
/// Handles the communication protocol for the ATEM mixers
/// </summary>
// TODO: Create interface
// TODO: Make IAsyncDisposable
public class AtemProtocol
{
    // TODO: public ConnectionStatus: Closed, Connecting, Connected, Disconnecting
    public ConnectionState ConnectionState { get; private set; } = ConnectionState.Closed;

    private ushort _nextSendPacketId = 1;
    private readonly Lock _nextSendPacketIdLock = new();
    private ushort _sessionId;

    private IPEndPoint _remoteEndpoint = new(IPAddress.None, Constants.AtemConstants.DEFAULT_PORT);
    private IUdpClient? _socket;

    private DateTime _lastReceivedAt = DateTime.Now;
    private ushort _lastReceivedPacketId;
    private List<InFlightPacket> _inflight = [];
    private CancellationTokenSource? _ackTimerCancellation;
    private int _receivedWithoutAck;
    private TaskCompletionSource? _connectionSource;

    public event EventHandler? Connected;
    public event EventHandler? Disconnected;

    public IReceivableSourceBlock<AtemPacket> ReceivedPackets => _receivedPackets;
    public IReceivableSourceBlock<int> AckedTrackingIds => _ackedTrackingIds;

    private ActionLoop? _receiveLoop;
    private BufferBlock<AtemPacket> _receivedPackets = new();
    private BufferBlock<int> _ackedTrackingIds = new();
    private ActionLoop? _reconnectTimer;
    private ActionLoop? _retransmitTimer;

    private void StartTimers()
    {
        _reconnectTimer ??= ActionLoop.Start(ReconnectTimerLoop);
        _retransmitTimer ??= ActionLoop.Start(RetransmitTimerLoop);
    }

    private async Task ReconnectTimerLoop(CancellationToken token)
    {
        await Task.Delay(ConnectionRetryInterval, token);

        if (_lastReceivedAt + ConnectionTimeout > DateTime.Now)
        {
            return;
        }

        RestartConnection().FireAndForget();
    }

    private async Task ClearTimers()
    {
        await Task.WhenAll(
            _reconnectTimer?.Cancel() ?? Task.CompletedTask,
            _retransmitTimer?.Cancel() ?? Task.CompletedTask
        );
    }

    private async Task RetransmitTimerLoop(CancellationToken token)
    {
        await Task.Delay(RetransmitInterval, token);

        CheckForRetransmit().FireAndForget();
    }

    public async Task Connect(IPEndPoint endPoint)
    {
        _remoteEndpoint = endPoint;

        await CreateSocket();

        _connectionSource = new();
        await RestartConnection();
        await _connectionSource.Task;
    }

    public async Task Disconnect()
    {
        await ClearTimers();
        await CloseSocket();

        _receivedPackets = new();
        _ackedTrackingIds = new();

        Debug.Print("Disconnected");
        ConnectionState = ConnectionState.Disconnected;

        OnDisconnected();
    }

    private async Task RestartConnection()
    {
        await ClearTimers();

        // This includes a 'disconnect'
        if (ConnectionState == ConnectionState.Established)
        {
            await RecreateSocket();
            OnDisconnected();
        } else if (ConnectionState == ConnectionState.Disconnected)
        {
            await CreateSocket();
        }

        // Reset connection
        _nextSendPacketId = 1;
        _sessionId = 0;

        // Try doing reconnect
        StartTimers();

        await SendPacket(CommandConnectHello);
        ConnectionState = ConnectionState.SynSent;
    }

    public async Task SendPackets(AtemPacket[] packets)
    {
        await Task.WhenAll(packets.Select(SendPacket).ToList());
    }

    private async Task SendPacket(AtemPacket packet)
    {
        using (var _ = _nextSendPacketIdLock.EnterScope())
        {
            packet.PacketId = _nextSendPacketId++;
            if (_nextSendPacketId >= MaxPacketId)
            {
                _nextSendPacketId = 0;
            }
        }
        packet.SessionId = _sessionId;

        await SendPacket(packet.ToBytes());
        _inflight.Add(new(packet.PacketId, packet.TrackingId, packet.Payload)
        {
            LastSent = DateTime.Now,
            Resent = 0
        });
    }

    private async Task RecreateSocket()
    {
        await CloseSocket();
        await CreateSocket();
    }

    private async Task CloseSocket()
    {
        await (_receiveLoop?.Cancel() ?? Task.CompletedTask);
        _receiveLoop = null;
        _socket?.Dispose();
    }

    private async Task CreateSocket()
    {
        if (_socket is not null) await CloseSocket();

        _socket = new UdpClientWrapper();
        _socket.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
        _socket.Connect(_remoteEndpoint);
        _receiveLoop = ActionLoop.Start(ReceiveLoopAsync);
    }

    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _socket!.ReceiveAsync(cancellationToken);
            await ReceivePacket(result.Buffer);
        }
        catch (TaskCanceledException)
        {
            // NOP
        }
    }

    private bool IsPacketCoveredByAck(ushort ackId, ushort packetId)
    {
        var tolerance = MaxPacketId / 2;
        var pktIsShortlyBefore = packetId < ackId && packetId + tolerance > ackId;
        var pktIsShortlyAfter = packetId > ackId && packetId < ackId + tolerance;
        var pktIsBeforeWrap = packetId > ackId + tolerance;
        return packetId == ackId || ((pktIsShortlyBefore || pktIsBeforeWrap) && !pktIsShortlyAfter);
    }

    private async Task ReceivePacket(byte[] buffer)
    {
        _lastReceivedAt = DateTime.Now;
        var packet = AtemPacket.FromBytes(buffer);

        if (packet.HasFlag(PacketFlag.NewSessionId))
        {
            Debug.Print("Connected");
            ConnectionState = ConnectionState.Established;
            _lastReceivedPacketId = packet.PacketId;
            _sessionId = packet.SessionId;
            await SendAck(packet.PacketId);
            OnConnected();
            return;
        }

        if (ConnectionState == ConnectionState.Established)
        {
            // Device asked for retransmit
            if (packet.HasFlag(PacketFlag.RetransmitRequest))
            {
                Debug.Print($"Retransmit request: {packet.RetransmitFromPacketId}");
                RetransmitFrom(packet.RetransmitFromPacketId).FireAndForget();
            }

            // Got a packet that needs an ack
            if (packet.HasFlag(PacketFlag.AckRequest))
            {
                // Check if it next in the sequence
                if (packet.PacketId == (_lastReceivedPacketId + 1) % MaxPacketId)
                {
                    _lastReceivedPacketId = packet.PacketId;
                    _sessionId = packet.SessionId;
                    await SendOrQueueAck();
                    if (packet.Payload.Length > 0)
                    {
                        await _receivedPackets.SendAsync(packet);
                    }
                } else if (IsPacketCoveredByAck(_lastReceivedPacketId, packet.PacketId))
                {
                    await SendOrQueueAck();
                }
            }

            // Device ack'ed our packet
            if (packet.HasFlag(PacketFlag.AckReply))
            {
                var ackPacketId = packet.AckPacketId;
                _inflight = _inflight.Where(pkt =>
                {
                    if (IsPacketCoveredByAck(ackPacketId, pkt.PacketId))
                    {
                        _ackedTrackingIds.SendAsync(pkt.TrackingId);
                        return false;
                    }
                    else
                    {
                        // Not acked yet
                        return true;
                    }
                }).ToList();
            }
        }
    }

    private async Task SendPacket(byte[] buffer)
    {
        await (_socket?.SendAsync(buffer) ?? Task.CompletedTask);
    }

    private async Task SendOrQueueAck()
    {
        _receivedWithoutAck++;
        if (_receivedWithoutAck >= MaxPacketPerAck)
        {
            await FireAckTimer();
        } else if (_ackTimerCancellation is null)
        {
            StartAckTimer();
        }
    }

    private async void StartAckTimer()
    {
        var cts = new CancellationTokenSource();
        _ackTimerCancellation = cts;
        try
        {
            await Task.Delay(TimeSpan.FromMilliseconds(5), cts.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        await FireAckTimer();
    }

    private async Task FireAckTimer()
    {
        _receivedWithoutAck = 0;
        _ackTimerCancellation?.Cancel();
        _ackTimerCancellation = null;
        await SendAck(_lastReceivedPacketId);
    }

    private async Task SendAck(ushort packetId)
    {
        await SendPacket(AtemPacket.CreateAck(_sessionId, packetId).ToBytes());
    }

    private async Task RetransmitFrom(ushort fromId)
    {
        // The atem will ask for MAX_PACKET_ID to be retransmitted when it really wants 0
        fromId = (ushort)(fromId % MaxPacketId);

        // TODO: Simplify using LINQ
        var fromIndex = _inflight.FindIndex(0, pkt => pkt.PacketId == fromId);
        if (fromIndex != -1)
        {
            Debug.Print($"Unable to resend: {fromId}: unknown packet ID");
            await RestartConnection();
        }
        else
        {
            var now = DateTime.Now;
            for (var currentIndex = fromIndex; currentIndex < _inflight.Count; currentIndex++)
            {
                var sentPacket =  _inflight[currentIndex];
                if (sentPacket.PacketId == fromId || !IsPacketCoveredByAck(fromId, sentPacket.PacketId))
                {
                    sentPacket.LastSent = now;
                    sentPacket.Resent++;
                    await SendPacket(sentPacket.Payload);
                }
            }
        }
    }

    private async Task CheckForRetransmit()
    {
        if (_inflight.Count == 0) return;

        var now = DateTime.Now;

        var foundPacket = _inflight.FirstOrDefault(sentPacket => sentPacket.LastSent + InFlightTimeout < now);
        if (!foundPacket.NonDefault) return;

        if (foundPacket.Resent <= MaxPacketRetries && IsPacketCoveredByAck(_nextSendPacketId, foundPacket.PacketId))
        {
            Debug.Print($"Retransmit from timeout: {foundPacket.PacketId}");
            await RetransmitFrom(foundPacket.PacketId);
        } else {
            Debug.Print($"Packet timed out {foundPacket.PacketId}");
            await RestartConnection();
        }
    }

    private static readonly TimeSpan ConnectionRetryInterval = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan RetransmitInterval = TimeSpan.FromMilliseconds(10);
    private static readonly byte[] CommandConnectHello =
    [
        0x10, 0x14, 0x53, 0xab, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3a, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00
    ];

    private static readonly ushort MaxPacketId = 1 << 15;
    private static readonly uint MaxPacketRetries = 10;
    private static readonly uint MaxPacketPerAck = 16;

    private static readonly TimeSpan InFlightTimeout = TimeSpan.FromMilliseconds(60);

    protected virtual void OnConnected()
    {
        _connectionSource?.TrySetResult();
        Connected?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnDisconnected()
    {
        Disconnected?.Invoke(this, EventArgs.Empty);
    }
}
