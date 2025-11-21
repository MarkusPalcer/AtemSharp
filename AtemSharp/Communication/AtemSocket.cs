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
    private Task _creatingSocket = Task.CompletedTask;
    private Action? _exitUnsubscribe = () => { };
    private ActionLoop? _receiveLoop;
    private ActionLoop? _ackLoop;

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

            _creatingSocket = CreateSocketProcess();
            await _creatingSocket;

            if (_isDisconnecting || _socketProcess is null)
            {
                throw new InvalidOperationException("Disconnecting");
            }
        }

        _receiveLoop = ActionLoop.Start(ReceivePacket);
        _ackLoop = ActionLoop.Start(DoAckLoop);
        await _socketProcess.Connect(_address, _port);
    }

    private async Task DoAckLoop(CancellationToken cts)
    {
        await _socketProcess.AckedTrackingIds.ReceiveAsync(cts);
        // TODO: Complete packet TCS'
    }

    public async ValueTask DisposeAsync()
    {
        await Disconnect();

        _exitUnsubscribe?.Invoke();
        _exitUnsubscribe = null;
    }

    public async Task Disconnect()
    {
        try
        {
            await _creatingSocket;
        }
        catch (Exception)
        {
            // Ignore Exceptions
        }

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

    private int GetNextTrackingId()
    {
        return Interlocked.Increment(ref _nextPacketTrackingId);
    }

    // TODO: make async and await all tracking IDs
    public int[] SendCommands(SerializedCommand[] commands)
    {
        if (_socketProcess is null) throw new InvalidOperationException("Socket process is not open");

        var packetBuilder = new PacketBuilder(_commandParser.Version);
        foreach (var command in commands)
        {
            packetBuilder.AddCommand(command);
        }

        var packets = packetBuilder.GetPackets().Select(buffer => new OutboundPacketInfo(buffer, GetNextTrackingId())).ToArray();

        if (packets.Length > 0)
        {
            _socketProcess.SendPackets(packets);
        }

        return packets.Select(x => x.TrackingId).ToArray();
    }

    // TODO: Move to constructor
    private async Task CreateSocketProcess()
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

    public Task SendCommand(SerializedCommand command, CancellationToken cancellationToken = default)
    {
        SendCommands([command]);
        return Task.CompletedTask;
    }

    protected virtual void OnPacketReceived(AtemPacket packet)
    {
        PacketReceived?.Invoke(this, new PacketReceivedEventArgs { Packet = packet});
    }
}
