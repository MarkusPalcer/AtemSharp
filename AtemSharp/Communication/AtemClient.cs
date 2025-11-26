using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Constants;
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
    private IAtemProtocol? _protocol;
    private ActionLoop? _receiveLoop;
    private ActionLoop? _ackLoop;
    private readonly ConcurrentDictionary<int, TaskCompletionSource> _commandAckSources = new();
    private BufferBlock<IDeserializedCommand> _receivedCommands = new();

    public IReceivableSourceBlock<IDeserializedCommand> ReceivedCommands => _receivedCommands;

    public async Task Connect(string address, int port)
    {
        _isDisconnecting = false;

        _remoteEndpoint = new IPEndPoint(IPAddress.Parse(address), port);

        if (_protocol is null)
        {
            _protocol = new AtemProtocol();

            if (_isDisconnecting || _protocol is null)
            {
                throw new InvalidOperationException("Disconnecting");
            }
        }

        _commandAckSources.Clear();
        _receivedCommands = new();
        _receiveLoop = ActionLoop.Start(DoReceivePacketLoop);
        _ackLoop = ActionLoop.Start(DoAckLoop);
        await _protocol.ConnectAsync(_remoteEndpoint);
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
                await _protocol.DisconnectAsync();
                await _protocol.DisposeAsync();
            }
            catch (Exception)
            {
                // Ignore Exceptions
            }

            _protocol = null;
        }
    }

    private async Task DoReceivePacketLoop(CancellationToken token)
    {
        var packet = await _protocol!.ReceivedPackets.ReceiveAsync(token);

        try
        {
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
                    if (command != null)
                    {
                        _receivedCommands.SendAsync(command, token).FireAndForget();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print($"Error while processing command: {ex.Message}\n{ex.StackTrace}");
                }

                // Move to next command
                offset += commandLength;
            }
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors during packet processing
            Debug.Print($"Error processing packet: {BitConverter.ToString(packet.Payload)}\n{ex.Message}\n{ex.StackTrace}");
        }
    }

    public async Task SendCommandAsync(SerializedCommand command)
    {
        await SendCommandsAsync([command]);
    }

    public async Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
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
            await _protocol.SendPacketsAsync(packets);
        }

        await Task.WhenAll(ackTcs.Select(t => t.Task));
    }

    public async Task ConnectAsync(string address, int port = AtemConstants.DefaultPort, CancellationToken cancellationToken = default)
    {
        await Connect(address, port);
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
    }
}
