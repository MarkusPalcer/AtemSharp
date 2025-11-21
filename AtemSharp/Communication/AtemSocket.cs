using System.Diagnostics;
using AtemSharp.Commands;
using AtemSharp.Constants;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

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

    public event EventHandler? Disconnected;

    public event EventHandler<ReceivedCommandsEventArgs>? ReceivedCommands;

    public event EventHandler<AckPacketsEventArgs>? AckPackets;

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

        await _socketProcess.Connect(_address, _port);
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

    private async Task CreateSocketProcess()
    {
        _socketProcess = new AtemSocketChild(_address,
                                             _port,
                                             () =>
                                             {
                                                 OnDisconnected();
                                                 return Task.CompletedTask;
                                             },
                                             (packet) =>
                                             {
                                                 OnPacketReceived(packet);
                                                 return Task.CompletedTask;
                                             },
                                             packets =>
                                             {
                                                 OnAckPackets(new AckPacketsEventArgs { PacketIds = packets.Select(x => x.TrackingId).ToArray() });
                                                 return Task.CompletedTask;
                                             });

        _socketProcess.Connected += HandleConnected;
    }

    private void HandleConnected(object? sender, EventArgs e)
    {
        OnConnected();
    }

    protected virtual void OnDisconnected()
    {
        Disconnected?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnAckPackets(AckPacketsEventArgs e)
    {
        AckPackets?.Invoke(this, e);
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
        // TODO: Complete task when command has been acked
        return Task.CompletedTask;
    }

    public Task SendPacketAsync(AtemPacket packet, CancellationToken cancellationToken = default)
    {
        var info = new OutboundPacketInfo(packet.ToBytes(), GetNextTrackingId());
        _socketProcess?.SendPackets([info]);
        return Task.CompletedTask;
    }

    public Task SendHelloPacketAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    protected virtual void OnPacketReceived(AtemPacket packet)
    {
        PacketReceived?.Invoke(this, new PacketReceivedEventArgs { Packet = packet});
    }
}
