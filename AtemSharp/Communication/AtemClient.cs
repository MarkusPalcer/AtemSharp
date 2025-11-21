using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Constants;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

/// <summary>
/// Sends and receives commands to a ATEM Mixer
/// </summary>
public class AtemClient : IAtemClient
{
    private readonly CommandParser _commandParser = new();

    private int _nextPacketTrackingId;
    private bool _isDisconnecting;
    private IPEndPoint _remoteEndpoint =  new(IPAddress.None, 0);
    private AtemProtocol? _protocol;
    private Action? _exitUnsubscribe = () => { };
    private ActionLoop? _receiveLoop;
    private ActionLoop? _ackLoop;
    private readonly ConcurrentDictionary<int, TaskCompletionSource> _commandAckSources = new();

    public event EventHandler? Disconnected;
    public event EventHandler<ReceivedCommandsEventArgs>? ReceivedCommands;
    public event EventHandler? Connected;
    public event EventHandler<PacketReceivedEventArgs>? PacketReceived;
    public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

    protected virtual void OnDisconnected()
    {
        Disconnected?.Invoke(this, EventArgs.Empty);
    }

    public async Task Connect(string address, int port)
    {
        _isDisconnecting = false;

        _remoteEndpoint = new IPEndPoint(IPAddress.Parse(address), port);

        if (_protocol is null)
        {
            CreateSocketProcess();

            if (_isDisconnecting || _protocol is null)
            {
                throw new InvalidOperationException("Disconnecting");
            }
        }

        _commandAckSources.Clear();
        _receiveLoop = ActionLoop.Start(ReceivePacket);
        _ackLoop = ActionLoop.Start(DoAckLoop);
        await _protocol.Connect(_remoteEndpoint);
    }

    private async Task DoAckLoop(CancellationToken cts)
    {
        var ackedId = await _protocol!.AckedTrackingIds.ReceiveAsync(cts);
        if (!_commandAckSources.Remove(ackedId, out var tcs)) return;
        tcs.TrySetResult();
    }

    public async Task DisconnectAsync()
    {
        await (_receiveLoop?.Cancel() ?? Task.CompletedTask);
        await (_ackLoop?.Cancel() ?? Task.CompletedTask);

        if (_protocol is not null)
        {
            try
            {
                await _protocol.Disconnect();
            }
            catch (Exception)
            {
                // Ignore Exceptions
            }

            _protocol = null;
        }
    }

    private void CreateSocketProcess()
    {
        _protocol = new AtemProtocol();

        _protocol.Connected += (_, _) => OnConnected();
        _protocol.Disconnected += (_, _) => OnDisconnected();
    }

    private async Task ReceivePacket(CancellationToken cts)
    {
        var packet = await _protocol!.ReceivedPackets.ReceiveAsync(cts);
        OnPacketReceived(packet);
    }


    public async Task SendCommands(SerializedCommand[] commands)
    {
        if (_protocol is null) throw new InvalidOperationException("Socket process is not open");

        var packetBuilder = new PacketBuilder(_commandParser.Version);
        foreach (var command in commands)
        {
            packetBuilder.AddCommand(command);
        }

        var packets = packetBuilder.GetPackets().Select(buffer => new AtemPacket(buffer)
        {
            TrackingId = Interlocked.Increment(ref _nextPacketTrackingId),
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
            await _protocol.SendPackets(packets);
        }

        await Task.WhenAll(ackTcs.Select(t => t.Task));
    }

    protected virtual void OnConnected()
    {
        Connected?.Invoke(this, EventArgs.Empty);
    }


    public async Task SendCommand(SerializedCommand command)
    {
        await SendCommands([command]);
    }

    public ConnectionState ConnectionState => _protocol?.ConnectionState ?? ConnectionState.Closed;

    public async Task ConnectAsync(string address, int port = AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default)
    {
        await Connect(address, port);
    }

    protected virtual void OnPacketReceived(AtemPacket packet)
    {
        PacketReceived?.Invoke(this, new PacketReceivedEventArgs { Packet = packet });
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();

        _exitUnsubscribe?.Invoke();
        _exitUnsubscribe = null;
    }
}
