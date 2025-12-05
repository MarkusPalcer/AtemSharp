using System.Diagnostics;
using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.DependencyInjection;
using AtemSharp.FrameworkAbstraction;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

/// <summary>
/// Handles the communication protocol for the ATEM mixers
/// </summary>
internal class AtemProtocol(IServices services) : IAtemProtocol
{
    private ConnectionState _connectionState = ConnectionState.Closed;
    private ushort _nextSendPacketId = 1;
    private readonly Lock _nextSendPacketIdLock = new();
    private ushort _sessionId;

    private IPEndPoint _remoteEndpoint = new(IPAddress.None, Constants.AtemConstants.DefaultPort);
    private IUdpClient? _udpClient;

    private DateTime _lastReceivedAt = services.TimeProvider.Now;
    private ushort _lastReceivedPacketId;
    private List<InFlightPacket> _inflight = [];
    private CancellationTokenSource? _ackTimerCancellation;
    private int _receivedWithoutAck;
    private TaskCompletionSource? _connectionSource;

    private IActionLoop? _receiveLoop;
    private BufferBlock<AtemPacket> _receivedPackets = new();
    private BufferBlock<int> _ackedTrackingIds = new();
    private IActionLoop? _reconnectTimer;
    private IActionLoop? _retransmitTimer;

    public IReceivableSourceBlock<AtemPacket> ReceivedPackets => _receivedPackets;
    public IReceivableSourceBlock<int> AckedTrackingIds => _ackedTrackingIds;


    private void StartTimers()
    {
        _reconnectTimer ??= services.StartActionLoop(ReconnectTimerLoop);
        _retransmitTimer ??= services.StartActionLoop(RetransmitTimerLoop);
    }

    private async Task ReconnectTimerLoop(CancellationToken token)
    {
        await services.TimeProvider.Delay(ConnectionRetryInterval, token);

        if (_lastReceivedAt + ConnectionTimeout > services.TimeProvider.Now)
        {
            return;
        }

        StartConnection().FireAndForget();
    }

    private async Task ClearTimers()
    {
        await Task.WhenAll(
            _reconnectTimer?.Cancel() ?? Task.CompletedTask,
            _retransmitTimer?.Cancel() ?? Task.CompletedTask
        );

        _reconnectTimer = null;
        _retransmitTimer = null;
    }

    private async Task RetransmitTimerLoop(CancellationToken token)
    {
        await services.TimeProvider.Delay(RetransmitInterval, token);

        CheckForRetransmit().FireAndForget();
    }

    public async Task ConnectAsync(IPEndPoint endPoint)
    {
        if (_connectionState != ConnectionState.Closed)
        {
            throw new InvalidOperationException("Can only connect while not connected");
        }

        _remoteEndpoint = endPoint;

        _connectionSource = new();
        await StartConnection();
        await _connectionSource.Task;
    }

    public async Task DisconnectAsync()
    {
        await ClearTimers();
        await (_ackTimerCancellation?.CancelAsync() ?? Task.CompletedTask);
        await CloseSocket();

        _receivedPackets = new();
        _ackedTrackingIds = new();

        _connectionState = ConnectionState.Closed;
    }

    private async Task StartConnection()
    {
        await ClearTimers();

        if (_udpClient is not null)
        {
            await CloseSocket();
        }

        CreateSocket();

        // Reset connection
        _nextSendPacketId = 1;
        _sessionId = 0;

        // Try doing reconnect
        StartTimers();

        await SendPacket(CommandConnectHello);
        _connectionState = ConnectionState.SynSent;
    }

    public async Task SendPacketsAsync(AtemPacket[] packets)
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
        _inflight.Add(new(packet.PacketId, packet.TrackingId, packet.ToBytes())
        {
            LastSent = services.TimeProvider.Now,
            Resent = 0
        });
    }

    private async Task CloseSocket()
    {
        await (_receiveLoop?.Cancel() ?? Task.CompletedTask);
        _receiveLoop = null;
        _udpClient?.Dispose();
    }

    private void CreateSocket()
    {
        _udpClient = services.CreateUdpClient();
        _udpClient.Bind(new IPEndPoint(IPAddress.Any, 0));
        _udpClient.Connect(_remoteEndpoint);
        _receiveLoop = services.StartActionLoop(ReceiveLoopAsync);
    }

    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _udpClient!.ReceiveAsync(cancellationToken);
            await ReceivePacket(result.Buffer);
        }
        catch (TaskCanceledException)
        {
            // NOP
        }
    }

    private static bool IsPacketCoveredByAck(ushort ackId, ushort packetId)
    {
        var tolerance = MaxPacketId / 2;
        var pktIsShortlyBefore = packetId < ackId && packetId + tolerance > ackId;
        var pktIsShortlyAfter = packetId > ackId && packetId < ackId + tolerance;
        var pktIsBeforeWrap = packetId > ackId + tolerance;
        return packetId == ackId || ((pktIsShortlyBefore || pktIsBeforeWrap) && !pktIsShortlyAfter);
    }

    private async Task ReceivePacket(byte[] buffer)
    {
        _lastReceivedAt = services.TimeProvider.Now;
        var packet = AtemPacket.FromBytes(buffer);

        if (packet.HasFlag(PacketFlag.NewSessionId))
        {
            _connectionState = ConnectionState.Established;
            _lastReceivedPacketId = packet.PacketId;
            _sessionId = packet.SessionId;
            await SendAck(packet.PacketId);
            _connectionSource?.TrySetResult();
            return;
        }

        if (_connectionState == ConnectionState.Established)
        {
            // Device asked for retransmit
            if (packet.HasFlag(PacketFlag.RetransmitRequest))
            {
                Debug.WriteLine($"Retransmit request from #{packet.RetransmitFromPacketId}");
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
        try
        {
            var sendTask = _udpClient?.SendAsync(buffer);
            if (sendTask != null)
            {
                await sendTask.Value;
            }
        }
        catch (ObjectDisposedException)
        {
            // NOP
            // This might happen when disconnecting when a racing condition happens
            // That causes a last packet to be sent after the socket is closed.
            // We can ignore this sent packet
        }
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
            await services.TimeProvider.Delay(AckDelay, cts.Token);
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
        await (_ackTimerCancellation?.CancelAsync()  ?? Task.CompletedTask);
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

        var fromIndex = _inflight.FindIndex(0, pkt => pkt.PacketId == fromId);
        if (fromIndex == -1)
        {
            Debug.WriteLine($"Unable to resend from #{fromId}: unknown packet ID");
            await StartConnection();
        }
        else
        {
            var now = services.TimeProvider.Now;
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
        if (_inflight.Count == 0)
        {
            return;
        }

        var now = services.TimeProvider.Now;

        var foundPacket = _inflight.FirstOrDefault(sentPacket => sentPacket.LastSent + InFlightTimeout < now);
        if (!foundPacket.NonDefault)
        {
            return;
        }

        if (foundPacket.Resent <= MaxPacketRetries && IsPacketCoveredByAck(_nextSendPacketId, foundPacket.PacketId))
        {
            Debug.WriteLine($"Retransmit from #{foundPacket.PacketId} timed out");
            await RetransmitFrom(foundPacket.PacketId);
        } else {
            Debug.WriteLine($"Packet #{foundPacket.PacketId} timed out");
            await StartConnection();
        }
    }

    internal static readonly TimeSpan ConnectionRetryInterval = TimeSpan.FromSeconds(1);
    internal static readonly TimeSpan ConnectionTimeout = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan RetransmitInterval = TimeSpan.FromMilliseconds(10);

    internal static readonly byte[] CommandConnectHello =
    [
        0x10, 0x14, 0x53, 0xab, 0x00, 0x00, 0x00, 0x00, 0x00, 0x3a, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        0x00
    ];

    public static readonly ushort MaxPacketId = 1 << 15;
    private static readonly uint MaxPacketRetries = 10;
    internal static readonly uint MaxPacketPerAck = 16;

    private static readonly TimeSpan InFlightTimeout = TimeSpan.FromMilliseconds(60);
    internal static readonly TimeSpan AckDelay = TimeSpan.FromMilliseconds(5);

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
    }
}
