using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Commands.DeviceProfile;
using AtemSharp.Constants;
using AtemSharp.DependencyInjection;
using AtemSharp.Lib;

namespace AtemSharp.Communication;

/// <summary>
/// Sends and receives commands to a ATEM Mixer
/// </summary>
internal class AtemClient(IServices services)
    : IAtemClient
{
    private readonly ICommandParser _commandParser = services.CreateCommandParser();
    private readonly PacketBuilder _packetBuilder = new();
    private readonly ConcurrentDictionary<int, TaskCompletionSource> _commandAckSources = new();
    private readonly SemaphoreSlim _sendLock = new(1);

    internal AtemSharp.ConnectionState State = AtemSharp.ConnectionState.Disconnected;
    private int _nextPacketTrackingId;
    private IPEndPoint _remoteEndpoint = new(IPAddress.None, 0);
    private IAtemProtocol? _protocol;
    private IActionLoop? _receiveLoop;
    private IActionLoop? _ackLoop;
    private BufferBlock<IDeserializedCommand> _receivedCommands = new();

    public IReceivableSourceBlock<IDeserializedCommand> ReceivedCommands => _receivedCommands;

    public async Task ConnectAsync(string address, int port = AtemConstants.DefaultPort)
    {
        switch (State)
        {
            case AtemSharp.ConnectionState.Connecting:
                throw new InvalidOperationException("Can't connect while already connecting");
            case AtemSharp.ConnectionState.Connected:
                throw new InvalidOperationException("Can't connect while already connected");
            case AtemSharp.ConnectionState.Disconnecting:
                throw new InvalidOperationException("Can't connect while disconnecting");
        }

        State = AtemSharp.ConnectionState.Connecting;

        _remoteEndpoint = new IPEndPoint(IPAddress.Parse(address), port);

        _receivedCommands = new BufferBlock<IDeserializedCommand>();

        _protocol = services.CreateAtemProtocol();
        await _protocol.ConnectAsync(_remoteEndpoint);

        _receiveLoop = services.StartActionLoop(ReceivePacketLoop);
        _ackLoop = services.StartActionLoop(AckLoop);

        State = AtemSharp.ConnectionState.Connected;
    }

    public async Task DisconnectAsync()
    {
        switch (State)
        {
            case AtemSharp.ConnectionState.Disconnected:
                throw new InvalidOperationException("Can't disconnect while not connected");
            case AtemSharp.ConnectionState.Connecting:
                throw new InvalidOperationException("Can't disconnect while connecting");
            case AtemSharp.ConnectionState.Disconnecting:
                throw new InvalidOperationException("Can't disconnect while already disconnecting");
        }

        State = AtemSharp.ConnectionState.Disconnecting;

        await _receiveLoop!.Cancel();
        await _ackLoop!.Cancel();

        try
        {
            await _protocol!.DisconnectAsync();
        }
        catch (Exception)
        {
            // Ignore Exceptions
        }

        await _protocol!.DisposeAsync();

        _protocol = null;

        foreach (var (_, tcs) in _commandAckSources)
        {
            tcs.TrySetCanceled();
        }

        State = AtemSharp.ConnectionState.Disconnected;
    }

    public async Task SendCommandAsync(SerializedCommand command)
    {
        await SendCommandsAsync([command]);
    }

    public async Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        await _sendLock.WaitAsync();

        try
        {
            await SendCommandsAsyncInternal(commands);
        }
        finally
        {
            _sendLock.Release();
        }
    }

    private async Task SendCommandsAsyncInternal(IEnumerable<SerializedCommand> commands)
    {
        if (State != AtemSharp.ConnectionState.Connected)
        {
            throw new InvalidOperationException("Cannot send data while not connected");
        }

        commands = commands.ToArray();

        _packetBuilder.ProtocolVersion = _commandParser.Version;
        foreach (var command in commands)
        {
            _packetBuilder.AddCommand(command);
        }

        var packets = _packetBuilder.GetPackets().Select(buffer => new AtemPacket(buffer)
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

        await _protocol!.SendPacketsAsync(packets);

        await Task.WhenAll(ackTcs.Select(t => t.Task));
    }

    private async Task AckLoop(CancellationToken cts)
    {
        var ackedId = await _protocol!.AckedTrackingIds.ReceiveAsync(cts);

        if (!_commandAckSources.Remove(ackedId, out var tcs))
        {
            return;
        }

        tcs.TrySetResult();
    }

    private async Task ReceivePacketLoop(CancellationToken token)
    {
        var packet = await _protocol!.ReceivedPackets.ReceiveAsync(token);

        // Extract commands from packet payload and apply to state
        // Based on TypeScript atemSocket.ts _parseCommands method (lines 181-217)
        var payload = packet.Payload;
        var offset = 0;

        // Parse all commands in the packet payload
        while (offset + AtemConstants.CommandHeaderSize <= payload.Length)
        {
            // Extract command header (8 bytes: length, reserved, rawName)
            var commandLength = (payload[offset] << 8) | payload[offset + 1]; // Big-endian 16-bit
            // Skip reserved bytes (offset + 2, offset + 3)
            var rawName = System.Text.Encoding.ASCII.GetString(payload, offset + 4, 4);

            // Validate command length
            if (commandLength < AtemConstants.CommandHeaderSize)
            {
                // Commands are never less than 8 bytes (header size)
                break;
            }

            if (offset + commandLength > payload.Length)
            {
                // Command extends beyond payload - malformed packet
                break;
            }

            // Extract command data (excluding the 8-byte header)
            var commandDataStart = offset + AtemConstants.CommandHeaderSize;
            var commandDataLength = commandLength - AtemConstants.CommandHeaderSize;
            var commandData = new Span<byte>(payload, commandDataStart, commandDataLength);

            try
            {
                // Try to parse the command using CommandParser
                var command = _commandParser.ParseCommand(rawName, commandData);
                if (command is VersionCommand versionCommand)
                {
                    _packetBuilder.ProtocolVersion = versionCommand.Version;
                    _commandParser.Version = versionCommand.Version;
                }

                if (command != null)
                {
                    _receivedCommands.SendAsync(command, token).FireAndForget();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while processing command\n{ex.Message}\n{ex.StackTrace}");
            }

            // Move to next command
            offset += commandLength;
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await DisconnectAsync();
        }
        catch (InvalidOperationException)
        {
            // NOP
        }
    }
}
