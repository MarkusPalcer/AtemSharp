using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.Lib;
using AtemSharp.State;

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

            _client = value;
            if (_receiveLoop is not null)
            {
                _receiveLoop = ActionLoop.Start(ReceiveCommandLoop);
            }
        }
    }

    private bool _disposed;
    private IAtemClient _client;
    private TaskCompletionSource<bool>? _connectionCompletionSource;
    private ActionLoop? _receiveLoop;



    public ConnectionState ConnectionState { get; private set; } = ConnectionState.Disconnected;

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
    public AtemSwitcher() : this(new AtemClient())
    {
    }

    /// <summary>
    /// Initializes a new instance of the Atem class with the specified transport
    /// This constructor is useful for testing scenarios where you want to inject a mock transport
    /// </summary>
    /// <param name="transport">The UDP transport to use for communication</param>
    /// <remarks>This constructor is solely for testing purposes to mock the IUdpTransport</remarks>
    internal AtemSwitcher(IAtemClient transport)
    {
        _client = transport ?? throw new ArgumentNullException(nameof(transport));
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

        if (ConnectionState != ConnectionState.Disconnected)
            throw new InvalidOperationException("Can not connect while not disconnected");

        ConnectionState = ConnectionState.Connecting;

        State = new AtemState();
        _connectionCompletionSource = new TaskCompletionSource<bool>();

        try
        {
            await Client.ConnectAsync(remoteHost, remotePort, cancellationToken);
            _receiveLoop = ActionLoop.Start(ReceiveCommandLoop);
            // Wait for InitCompleteCommand to be received, indicating the connection is fully established
            await _connectionCompletionSource.Task.WaitAsync(cancellationToken);
            ConnectionState = ConnectionState.Connected;
        }
        catch (Exception)
        {
            ConnectionState = ConnectionState.Disconnected;
            _receiveLoop?.Cancel();
            _receiveLoop = null;
            throw;
        }
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
        if (_disposed)
        {
            return;
        }

        // Disconnecting is idempotent
        switch (ConnectionState)
        {
            case ConnectionState.Connecting:
            case ConnectionState.Disconnecting:
                throw new InvalidOperationException("Can not disconnect while transitioning connection states");
            case ConnectionState.Disconnected:
                return; // Disconnecting is idempotent
            case ConnectionState.Connected:
                ConnectionState = ConnectionState.Disconnecting;
                try
                {
                    await Client.DisconnectAsync();
                }
                catch (Exception)
                {
                    ConnectionState = ConnectionState.Connected;
                    throw;
                }
                break;
        }
    }

    public async Task SendCommandAsync(SerializedCommand command)
    {
        await Client.SendCommandAsync(command);
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

        await Client.DisposeAsync();

        // Clean up any pending connection completion
        _connectionCompletionSource?.TrySetCanceled();
    }

    /* TODO: Abstract macro handling:
     * - When executing a task is returned that returns when the macro is finished executing
     * - Cancellation causes the macro execution to be cancelled
     * - Multiple macros are either queued (default) or the currently executed macro is cancelled and the new one started (optional parameter)
     */
}

public enum ConnectionState
{
    Disconnected,
    Connecting,
    Connected,
    Disconnecting
}
