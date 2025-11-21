using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

/// <summary>
/// Handles the communication protocol for the ATEM mixers
/// </summary>
// TODO: Create interface
// TODO: Make IAsyncDisposable
public class AtemSocketChild
{
    // TODO: public ConnectionStatus: Closed, Connecting, Connected, Disconnecting
    public ConnectionState ConnectionState { get; private set; } = ConnectionState.Closed;

    private ushort _nextSendPacketId = 1;
    private ushort _sessionId;

    // TODO: Turn into IPAddress instance
    private string _address = "127.0.0.1";
    private int _port = Constants.AtemConstants.DEFAULT_PORT;
    private IUdpClient? _socket;

    private DateTime _lastReceivedAt = DateTime.Now;
    private ushort _lastReceivedPacketId;
    private List<InFlightPacket> _inflight = [];
    private CancellationTokenSource? _ackTimerCancellation;
    private int _receivedWithoutAck;

    public event EventHandler? Connected;
    public event EventHandler? Disconnected;

    public IReceivableSourceBlock<AtemPacket> ReceivedPackets => _receivedPackets;
    public IReceivableSourceBlock<int> AckedTrackingIds => _ackedTrackingIds;

    private Task? _receiveLoop;
    private CancellationTokenSource? _connectionTokenSource;
    private BufferBlock<AtemPacket> _receivedPackets = new();
    private BufferBlock<int> _ackedTrackingIds = new();
    private ActionLoop? _reconnectTimer;
    private ActionLoop? _retransmitTimer;

    private void StartTimers()
    {
        _reconnectTimer ??= ActionLoop.Start(ReconnectTimer);
        _retransmitTimer ??= ActionLoop.Start(RetransmitTimer);
    }

    private async Task ReconnectTimer(CancellationToken token)
    {
        await Task.Delay(ConnectionRetryInterval, token);

        if (_lastReceivedAt + ConnectionTimeout > DateTime.Now)
        {
            return;
        }

        RestartConnection().ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Log($"Reconnect failed: {t.Exception}");
            }
        });
    }

    private async Task RetransmitTimer(CancellationToken token)
    {
        await Task.Delay(RetransmitInterval, token);

        CheckForRetransmit().ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Log($"Failed to retransmit {t.Exception}");
            }
        });
    }

    // TODO: Make async task
    private async void ClearTimers()
    {
        await Task.WhenAll(
            _reconnectTimer?.Cancel() ?? Task.CompletedTask,
            _retransmitTimer?.Cancel() ?? Task.CompletedTask
        );
    }

    // TODO: Await connection event
    public async Task Connect(string address, int port)
    {
        _address = address;
        _port = port;

        CreateSocket();
        await RestartConnection();
    }

    // TODO: Await timer finalization and socket destruction
    public async Task Disconnect()
    {
        ClearTimers();
        CloseSocket();

        _receivedPackets = await _receivedPackets.Recreate();
        _ackedTrackingIds = await _ackedTrackingIds.Recreate();

        Log("Disconnected");
        ConnectionState = ConnectionState.Disconnected;

        OnDisconnected();
    }

    private async Task RestartConnection()
    {
        ClearTimers();

        // This includes a 'disconnect'
        if (ConnectionState == ConnectionState.Established)
        {
            RecreateSocket();
            OnDisconnected();
        } else if (ConnectionState == ConnectionState.Disconnected)
        {
            CreateSocket();
        }

        // Reset connection
        _nextSendPacketId = 1;
        _sessionId = 0;
        Log("Reconnect");

        // Try doing reconnect
        StartTimers();

        SendPacket(CommandConnectHello);
        ConnectionState = ConnectionState.SynSent;
        Log("Syn Sent");

        // TODO: Await connection event
    }

    private void Log(string message)
    {
        Debug.Print(message);
    }

    public void SendPackets(OutboundPacketInfo[] packets)
    {
        foreach (var packet in packets)
        {
            SendPacket((ushort)packet.Payload.Length, packet.Payload, packet.TrackingId);
        }
    }

    private void SendPacket(ushort payloadLength, byte[] payload, int trackingId)
    {
        // TODO: Use concurrent counter
        ushort packetId = _nextSendPacketId++;
        if (_nextSendPacketId >= MaxPacketId)
        {
            _nextSendPacketId = 0;
        }

        // TODO: Replace with AtemPacket serialization
        ushort opCode = ((ushort)PacketFlag.AckRequest << 11);
        var flagsAndLength = (ushort)(opCode | (payloadLength + 12));
        var buffer = new byte[12 + payloadLength];

        buffer.WriteUInt16BigEndian(flagsAndLength, 0);
        buffer.WriteUInt16BigEndian(_sessionId, 2);
        buffer.WriteUInt16BigEndian(packetId, 10);
        Array.Copy(payload, 0, buffer, 12, payloadLength);
        SendPacket(buffer);
        _inflight.Add(new(packetId, trackingId, payload)
        {
            LastSent = DateTime.Now,
            Resent = 0
        });
    }

    private void RecreateSocket()
    {
        CloseSocket();
        CreateSocket();
    }

    private void CloseSocket()
    {
        _socket?.Dispose();
        _connectionTokenSource?.Cancel();
        _connectionTokenSource?.Dispose();
        _connectionTokenSource = null;

        (_receiveLoop ?? Task.CompletedTask).Wait();
    }

    private void CreateSocket()
    {
        _connectionTokenSource = new();

        _socket?.Dispose();
        _socket = ((Func<IUdpClient>)(() => new UdpClientWrapper()))();
        _socket.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
        _socket.Connect(new IPEndPoint(IPAddress.Parse(_address), _port));
        _receiveLoop = ReceiveLoopAsync(_connectionTokenSource.Token);
    }

    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            UdpReceiveResult result;
            try
            {
                result = await _socket!.ReceiveAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested) return;

                Log($"Error receiving: {ex.Message}\n Cancelling Receive-Loop and waiting for reconnect");
                return;
            }

            ReceivePacket(result.Buffer);
        }
        Log("Receive loop exited");
    }

    private bool IsPacketCoveredByAck(ushort ackId, ushort packetId)
    {
        var tolerance = MaxPacketId / 2;
        var pktIsShortlyBefore = packetId < ackId && packetId + tolerance > ackId;
        var pktIsShortlyAfter = packetId > ackId && packetId < ackId + tolerance;
        var pktIsBeforeWrap = packetId > ackId + tolerance;
        return packetId == ackId || ((pktIsShortlyBefore || pktIsBeforeWrap) && !pktIsShortlyAfter);
    }

    private void ReceivePacket(byte[] buffer)
    {
        Log($"AtemSocketChild: RECV {BitConverter.ToString(buffer)}");

        _lastReceivedAt = DateTime.Now;
        var packet = AtemPacket.FromBytes(buffer);

        Log($"AtemSocketChild: Packet #{packet.PacketId}, Flags: {packet.Flags}, SessionId: {packet.SessionId}");

        if (packet.HasFlag(PacketFlag.NewSessionId))
        {
            Log("Connected");
            ConnectionState = ConnectionState.Established;
            _lastReceivedPacketId = packet.PacketId;
            _sessionId = packet.SessionId;
            SendAck(packet.PacketId);
            OnConnected();
            return;
        }

        List<Task> ps = new();

        if (ConnectionState == ConnectionState.Established)
        {
            // Device asked for retransmit
            if (packet.HasFlag(PacketFlag.RetransmitRequest))
            {
                Log($"Retransmit request: {packet.RetransmitFromPacketId}");
                ps.Add(RetransmitFrom(packet.RetransmitFromPacketId));
            }

            // Got a packet that needs an ack
            if (packet.HasFlag(PacketFlag.AckRequest))
            {
                // Check if it next in the sequence
                if (packet.PacketId == (_lastReceivedPacketId + 1) % MaxPacketId)
                {
                    _lastReceivedPacketId = packet.PacketId;
                    _sessionId = packet.SessionId;
                    SendOrQueueAck();
                    if (packet.Payload.Length > 0)
                    {
                        _receivedPackets.SendAsync(packet);
                    }
                } else if (IsPacketCoveredByAck(_lastReceivedPacketId, packet.PacketId))
                {
                    SendOrQueueAck();
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

        Task.WhenAll(ps).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                Log($"AtemSocketChild: Failed to ReceivePacket: {t.Exception.Message}");
            }
            else
            {
                Log("Packet processed");
            }
        });
    }

    // TODO: Make async Task and handle errors
    private void SendPacket(byte[] buffer)
    {
        Log($"AtemSocketChild: SEND {BitConverter.ToString(buffer)}");
        _socket?.SendAsync(buffer, _connectionTokenSource?.Token ?? CancellationToken.None);
    }

    private void SendOrQueueAck()
    {
        _receivedWithoutAck++;
        if (_receivedWithoutAck >= MaxPacketPerAck)
        {
            Log("AckTimer short circuit due to too many received packets");
            FireAckTimer();
        } else if (_ackTimerCancellation is null)
        {
            Log("AckTimer started");
            StartAckTimer();
        }
        else
        {
            Log("AckTimer already running");
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

        FireAckTimer();
    }

    private void FireAckTimer()
    {
        _receivedWithoutAck = 0;
        _ackTimerCancellation?.Cancel();
        _ackTimerCancellation = null;
        SendAck(_lastReceivedPacketId);
    }

    private void SendAck(ushort packetId)
    {
        Log($"Sending Ack for #{packetId}, SessionId: {_sessionId}");
        SendPacket(AtemPacket.CreateAck(_sessionId, packetId).ToBytes());
    }

    private async Task RetransmitFrom(ushort fromId)
    {
        // The atem will ask for MAX_PACKET_ID to be retransmitted when it really wants 0
        fromId = (ushort)(fromId % MaxPacketId);

        // TODO: Simplify using LINQ
        var fromIndex = _inflight.FindIndex(0, pkt => pkt.PacketId == fromId);
        if (fromIndex != -1)
        {
            Log($"Unable to resend: {fromId}");
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
                    SendPacket(sentPacket.Payload);
                }
            }
        }
    }

    private async Task CheckForRetransmit()
    {
        if (_inflight.Count == 0) return;

        var now = DateTime.Now;
        // TODO: Simplify using LINQ (.First ...)
        foreach (var sentPacket in _inflight)
        {
            if (sentPacket.LastSent + InFlightTimeout < now)
            {
                if (sentPacket.Resent <= MaxPacketRetries && IsPacketCoveredByAck(_nextSendPacketId, sentPacket.PacketId))
                {
                    Log($"Retransmit from timeout: {sentPacket.PacketId}");
                    await RetransmitFrom(sentPacket.PacketId);
                    return;
                } else {
                    Log($"Packet timed out {sentPacket.PacketId}");
                    await RestartConnection();
                    return;
                }
            }
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
        Connected?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnDisconnected()
    {
        Disconnected?.Invoke(this, EventArgs.Empty);
    }
}
