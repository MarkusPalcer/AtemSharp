using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.Lib;
using AtemSharp.State;
using Microsoft.Extensions.Logging;

namespace AtemSharp;

public class AtemMixer : IDisposable
{
    private readonly CommandParser _commandParser = new();

    internal IUdpTransport Transport
    {
        get => _transport;
        set
        {
            // Subscribe to transport events
            _transport.PacketReceived -= OnPacketReceived;
            _transport.ConnectionStateChanged -= OnConnectionStateChanged;
            _transport.ErrorOccurred -= OnErrorOccurred;

            value.PacketReceived += OnPacketReceived;
            value.ConnectionStateChanged += OnConnectionStateChanged;
            value.ErrorOccurred += OnErrorOccurred;
            _transport = value;
        }
    }

    private readonly ILogger<AtemMixer> _logger;
    private bool _disposed;
    private IUdpTransport _transport;
    private TaskCompletionSource<bool>? _connectionCompletionSource;

    /// <summary>
    /// Gets the current ATEM state
    /// </summary>
    public AtemState? State { get; private set; }

    /// <summary>
    /// Gets a collection of unknown command raw names encountered during communication
    /// </summary>
    public static HashSet<string> UnknownCommands { get; } = new();

    /// <summary>
    /// Initializes a new instance of the Atem class
    /// </summary>
    /// <param name="logger">Logger instance for diagnostic output</param>
    public AtemMixer(ILogger<AtemMixer>? logger = null) : this(new AtemClient(), logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the Atem class with the specified transport
    /// This constructor is useful for testing scenarios where you want to inject a mock transport
    /// </summary>
    /// <param name="transport">The UDP transport to use for communication</param>
    /// <param name="logger">Logger instance for diagnostic output</param>
    /// <remarks>This constructor is solely for testing purposes to mock the IUdpTransport</remarks>
    internal AtemMixer(IUdpTransport transport, ILogger<AtemMixer>? logger = null)
    {
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<AtemMixer>.Instance;
        _transport.PacketReceived += OnPacketReceived;
        _transport.ConnectionStateChanged += OnConnectionStateChanged;
        _transport.ErrorOccurred += OnErrorOccurred;
    }

    /// <summary>
    /// Connects to an ATEM device
    /// </summary>
    /// <param name="remoteHost">IP address of the ATEM device</param>
    /// <param name="remotePort">Port number (default: 9910)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task that completes when connected</returns>
    public async Task ConnectAsync(string remoteHost, int remotePort = Constants.AtemConstants.DEFAULT_PORT,
                                   CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(AtemMixer));

        State = new AtemState();
        _connectionCompletionSource = new TaskCompletionSource<bool>();

        await Transport.ConnectAsync(remoteHost, remotePort, cancellationToken);

        // Wait for InitCompleteCommand to be received, indicating the connection is fully established
        await _connectionCompletionSource.Task.WaitAsync(cancellationToken);
    }

    /// <summary>
    /// Disconnects from the ATEM device
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task that completes when disconnected</returns>
    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (!_disposed)
        {
            await Transport.DisconnectAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Gets the current connection state
    /// </summary>
    public Enums.ConnectionState ConnectionState => Transport.ConnectionState;

    // TODO: Move command deserialization to AtemSocket
    private void OnPacketReceived(object? sender, PacketReceivedEventArgs e)
    {
        try
        {
            // Extract commands from packet payload and apply to state
            // Based on TypeScript atemSocket.ts _parseCommands method (lines 181-217)
            var payload = e.Packet.Payload;
            var offset = 0;

            // Parse all commands in the packet payload
            while (offset + Constants.AtemConstants.COMMAND_HEADER_SIZE <= payload.Length)
            {
                // Extract command header (8 bytes: length, reserved, rawName)
                var commandLength = (payload[offset] << 8) | payload[offset + 1]; // Big-endian 16-bit
                // Skip reserved bytes (offset + 2, offset + 3)
                var rawName = System.Text.Encoding.ASCII.GetString(payload, offset + 4, 4);

                // Validate command length
                if (commandLength < Constants.AtemConstants.COMMAND_HEADER_SIZE)
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
                var commandDataStart = offset + Constants.AtemConstants.COMMAND_HEADER_SIZE;
                var commandDataLength = commandLength - Constants.AtemConstants.COMMAND_HEADER_SIZE;
                var commandData = new Span<Byte>(payload, commandDataStart, commandDataLength);

                try
                {
                    // Try to parse the command using CommandParser
                    var command = _commandParser.ParseCommand(rawName, commandData);
                    if (command != null)
                    {
                        // Apply the command to the current state
                        command.ApplyToState(State!);

                        // Check if this is the InitCompleteCommand
                        if (command is Commands.InitCompleteCommand)
                        {
                            // Signal that the connection is fully established (only once)
                            _connectionCompletionSource?.TrySetResult(true);
                        }
                    }
                    // Note: Unknown commands are tracked by CommandParser.ParseCommand
#if DEBUG
					_logger.LogDebug("Processed command: {CommandName}", rawName);
#endif
                }
                catch (Exception ex)
                {
                    // Log command parsing error but continue processing other commands
                    // Matches TypeScript emit('error', `Failed to deserialize command: ${cmdConstructor.constructor.name}: ${e}`)
                    _logger.LogError(ex, "Failed to deserialize command {CommandName}: {ErrorMessage}", rawName, ex.Message);
                }

                // Move to next command
                offset += commandLength;
            }
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors during packet processing
            _logger.LogError(ex, "Error processing packet: {ErrorMessage}", ex.Message);
        }
    }

    private void OnConnectionStateChanged(object? sender, ConnectionStateChangedEventArgs e)
    {
        // Handle connection state transitions
        // This is primarily driven by the transport layer (UDP handshake)
        // but may be further refined by command processing (e.g., InitComplete)

        _logger.LogInformation("Connection state changed: {PreviousState} -> {NewState}", e.PreviousState, e.State);
    }

    private void OnErrorOccurred(object? sender, Exception e)
    {
        // Handle transport layer errors
        _logger.LogError(e, "Transport error occurred: {ErrorMessage}", e.Message);
    }

    /// <summary>
    /// Disposes the Atem instance and releases all resources
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        Transport.PacketReceived -= OnPacketReceived;
        Transport.ConnectionStateChanged -= OnConnectionStateChanged;
        Transport.ErrorOccurred -= OnErrorOccurred;
        Transport.Dispose();

        // Clean up any pending connection completion
        _connectionCompletionSource?.TrySetCanceled();
    }

    public async Task SendCommand(SerializedCommand command)
    {
        await Transport.SendCommand(command);
    }

    /* TODO: Abstract macro handling:
     * - When executing a task is returned that returns when the macro is finished executing
     * - Cancellation causes the macro execution to be cancelled
     * - Multiple macros are either queued (default) or the currently executed macro is cancelled and the new one started (optional parameter)
     */
}
