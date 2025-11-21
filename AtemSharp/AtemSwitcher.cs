using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.Lib;
using AtemSharp.State;
using Microsoft.Extensions.Logging;

namespace AtemSharp;

/// <summary>
/// Represents an ATEM switcher that is connected
/// </summary>
public class AtemSwitcher : IAsyncDisposable
{
    internal IAtemClient Client
    {
        get => _client;
        set
        {
            // Subscribe to transport events
            _receiveLoop?.Cancel().FireAndForget();
            _client.ConnectionStateChanged -= OnConnectionStateChanged;

            value.ConnectionStateChanged += OnConnectionStateChanged;
            _client = value;
            if (_receiveLoop is not null)
            {
                _receiveLoop = ActionLoop.Start(ReceiveCommandLoop);
            }
        }
    }

    private readonly ILogger<AtemSwitcher> _logger;
    private bool _disposed;
    private IAtemClient _client;
    private TaskCompletionSource<bool>? _connectionCompletionSource;
    private ActionLoop? _receiveLoop;

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
    public AtemSwitcher(ILogger<AtemSwitcher>? logger = null) : this(new AtemClient(), logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the Atem class with the specified transport
    /// This constructor is useful for testing scenarios where you want to inject a mock transport
    /// </summary>
    /// <param name="transport">The UDP transport to use for communication</param>
    /// <param name="logger">Logger instance for diagnostic output</param>
    /// <remarks>This constructor is solely for testing purposes to mock the IUdpTransport</remarks>
    internal AtemSwitcher(IAtemClient transport, ILogger<AtemSwitcher>? logger = null)
    {
        _client = transport ?? throw new ArgumentNullException(nameof(transport));
        _logger = logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger<AtemSwitcher>.Instance;
        _client.ConnectionStateChanged += OnConnectionStateChanged;
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
            throw new ObjectDisposedException(nameof(AtemSwitcher));

        State = new AtemState();
        _connectionCompletionSource = new TaskCompletionSource<bool>();

        await Client.ConnectAsync(remoteHost, remotePort, cancellationToken);
        _receiveLoop = ActionLoop.Start(ReceiveCommandLoop);
        // Wait for InitCompleteCommand to be received, indicating the connection is fully established
        await _connectionCompletionSource.Task.WaitAsync(cancellationToken);
    }

    private async Task ReceiveCommandLoop(CancellationToken token)
    {
        var command = await _client.ReceivedCommands.ReceiveAsync(token);

        // Apply the command to the current state
        command.ApplyToState(State!);

        // Check if this is the InitCompleteCommand
        if (command is InitCompleteCommand)
        {
            // Signal that the connection is fully established (only once)
            _connectionCompletionSource?.TrySetResult(true);
        }
    }

    /// <summary>
    /// Disconnects from the ATEM device
    /// </summary>
    /// <returns>Task that completes when disconnected</returns>
    public async Task DisconnectAsync()
    {
        if (!_disposed)
        {
            await Client.DisconnectAsync();
        }
    }

    /// <summary>
    /// Gets the current connection state
    /// </summary>
    public Enums.ConnectionState ConnectionState => Client.ConnectionState;

    private void OnConnectionStateChanged(object? sender, ConnectionStateChangedEventArgs e)
    {
        // Handle connection state transitions
        // This is primarily driven by the transport layer (UDP handshake)
        // but may be further refined by command processing (e.g., InitComplete)

        _logger.LogInformation("Connection state changed: {PreviousState} -> {NewState}", e.PreviousState, e.State);
    }

    /// <summary>
    /// Disposes the Atem instance and releases all resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (_receiveLoop != null) await _receiveLoop.Cancel();

        Client.ConnectionStateChanged -= OnConnectionStateChanged;
        await Client.DisposeAsync();

        // Clean up any pending connection completion
        _connectionCompletionSource?.TrySetCanceled();
    }

    public async Task SendCommand(SerializedCommand command)
    {
        await Client.SendCommand(command);
    }

    /* TODO: Abstract macro handling:
     * - When executing a task is returned that returns when the macro is finished executing
     * - Cancellation causes the macro execution to be cancelled
     * - Multiple macros are either queued (default) or the currently executed macro is cancelled and the new one started (optional parameter)
     */
}
