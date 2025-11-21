using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Constants;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

/// <summary>
/// Sends and receives commands to a ATEM Mixer
/// </summary>
public class AtemSocket : IAtemSocket, IUdpTransport
{
    private readonly CommandParser _commandParser = new();

    private int _nextPacketTrackingId;
    private bool _isDisconnecting;
    private string _address = "127.0.0.1";
    private int _port;
    private AtemSocketChild? _socketProcess;
    private Action? _exitUnsubscribe = () => { };
    private ActionLoop? _receiveLoop;
    private ActionLoop? _ackLoop;
    private readonly ConcurrentDictionary<int, TaskCompletionSource> _commandAckSources = new();

    public event EventHandler? Disconnected;

    public event EventHandler<ReceivedCommandsEventArgs>? ReceivedCommands;

    public event EventHandler? Connected;

    public async Task Connect(string address, int port)
    {
        _isDisconnecting = false;

        _address = address;
        _port = port;

        if (_socketProcess is null)
        {
            CreateSocketProcess();

            if (_isDisconnecting || _socketProcess is null)
            {
                throw new InvalidOperationException("Disconnecting");
            }
        }

        _commandAckSources.Clear();
        _receiveLoop = ActionLoop.Start(ReceivePacket);
        _ackLoop = ActionLoop.Start(DoAckLoop);
        await _socketProcess.Connect(_address, _port);
    }

    private async Task DoAckLoop(CancellationToken cts)
    {
        var ackedId = await _socketProcess.AckedTrackingIds.ReceiveAsync(cts);
        if (!_commandAckSources.Remove(ackedId, out var tcs)) return;
        tcs.TrySetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await Disconnect();

        _exitUnsubscribe?.Invoke();
        _exitUnsubscribe = null;
    }

    public async Task Disconnect()
    {
        await (_receiveLoop?.Cancel() ?? Task.CompletedTask);
        await (_ackLoop?.Cancel() ?? Task.CompletedTask);

        if (_socketProcess is not null)
        {
            try
            {
                await _socketProcess.Disconnect();
            }
            catch (Exception)
            {
                // Ignore Exceptions
            }

            _socketProcess = null;
        }
    }

    // TODO: Inline
    private int GetNextTrackingId()
    {
        return Interlocked.Increment(ref _nextPacketTrackingId);
    }


    public async Task SendCommands(SerializedCommand[] commands)
    {
        if (_socketProcess is null) throw new InvalidOperationException("Socket process is not open");

        var packetBuilder = new PacketBuilder(_commandParser.Version);
        foreach (var command in commands)
        {
            packetBuilder.AddCommand(command);
        }

        var packets = packetBuilder.GetPackets().Select(buffer => new AtemPacket(buffer)
        {
            TrackingId = GetNextTrackingId(),
            Flags = PacketFlag.AckRequest
        }).ToArray();

        var ackTcs = new List<TaskCompletionSource>(packets.Length);

        foreach (var outboundPacketInfo in packets)
        {
            var taskCompletionSource = new TaskCompletionSource();
            _commandAckSources[outboundPacketInfo.TrackingId] = taskCompletionSource;
            ackTcs.Add(taskCompletionSource);
        }

        if (packets.Length > 0)
        {
            _socketProcess.SendPackets(packets);
        }

        await Task.WhenAll(ackTcs.Select(t => t.Task));
    }

    private void CreateSocketProcess()
    {
        _socketProcess = new AtemSocketChild();

        _socketProcess.Connected += (_,_) => OnConnected();
        _socketProcess.Disconnected += (_,_) => OnDisconnected();
    }

    private async Task ReceivePacket(CancellationToken cts)
    {
        var packet = await _socketProcess!.ReceivedPackets.ReceiveAsync(cts);
        OnPacketReceived(packet);
    }

    protected virtual void OnDisconnected()
    {
        Disconnected?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnConnected()
    {
        Connected?.Invoke(this, EventArgs.Empty);
    }

    public void Dispose()
    {
        DisposeAsync().AsTask().Wait();
    }

    public event EventHandler<PacketReceivedEventArgs>? PacketReceived;
    public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;
    public event EventHandler<Exception>? ErrorOccurred;

    public ConnectionState ConnectionState => _socketProcess?.ConnectionState ?? ConnectionState.Closed;

    public async Task ConnectAsync(string address, int port = AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default)
    {
        await Connect(address, port);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        await Disconnect();
    }

    // TODO: Handle cancellation
    public async Task SendCommand(SerializedCommand command, CancellationToken cancellationToken = default)
    {
        await SendCommands([command]);
    }

    protected virtual void OnPacketReceived(AtemPacket packet)
    {
        PacketReceived?.Invoke(this, new PacketReceivedEventArgs { Packet = packet});
    }
}

