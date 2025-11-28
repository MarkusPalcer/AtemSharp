using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.Lib;
using AtemSharp.Logging;
using AtemSharp.State;
using Microsoft.Extensions.Logging;

namespace AtemSharp;

/// <summary>
/// Represents an ATEM switcher
/// </summary>
public class AtemSwitcher : IAtemSwitcher
{
    private bool _disposed;
    private readonly string _remoteHost;
    private readonly int _remotePort;
    private readonly IAtemClient _client;
    private TaskCompletionSource<bool>? _connectionCompletionSource;
    private ActionLoop? _receiveLoop;
    private ConnectionState _connectionState = ConnectionState.Disconnected;
    private ILogger<AtemSwitcher> _logger;

    /// <summary>
    /// Fires, when the value of <see cref="ConnectionState"/> has changed
    /// </summary>
    public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

    /// <summary>
    /// The state of the connection to the ATEM switcher
    /// </summary>
    public ConnectionState ConnectionState
    {
        get => _connectionState;
        private set
        {
            var oldValue = _connectionState;
            _connectionState = value;
            OnConnectionStateChanged(oldValue, value);
        }
    }

    /// <summary>
    /// Gets the current ATEM state
    /// </summary>
    public AtemState State { get; private set; } = new();

    /// <summary>
    /// Gets a collection of unknown command raw names encountered during communication
    /// </summary>
    public static HashSet<string> UnknownCommands { get; } = new();

    /// <summary>
    /// Initializes a new instance of the Atem class
    /// </summary>
    /// <param name="remoteHost">IP address of the ATEM device</param>
    /// <param name="remotePort">Port number (default: 9910)</param>
    /// <param name="loggerFactory">A logger factory to support logging. If omitted, logging happens via Debug.WriteLine</param>
    public AtemSwitcher(string remoteHost, int remotePort = Constants.AtemConstants.DefaultPort, ILoggerFactory? loggerFactory = null)
        : this(remoteHost, remotePort, null, loggerFactory)
    {
    }

    // internal constructor for passing in mocked AtemClient during tests
    internal AtemSwitcher(string remoteHost, int remotePort, IAtemClient? transport, ILoggerFactory? loggerFactory)
    {
        _remoteHost = remoteHost;
        _remotePort = remotePort;
        loggerFactory ??= new DebugLoggerFactory();
        _client = transport ?? new AtemClient(loggerFactory);
        _logger =  loggerFactory.CreateLogger<AtemSwitcher>();
    }

    /// <summary>
    /// Connects to an ATEM device
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task that completes when connected</returns>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
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
            await _client.ConnectAsync(_remoteHost, _remotePort, cancellationToken);
            _receiveLoop = ActionLoop.Start(ReceiveCommandLoop, _logger);

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
        command.ApplyToState(State);

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
                    await _client.DisconnectAsync();
                    ConnectionState = ConnectionState.Disconnected;
                }
                catch (Exception)
                {
                    ConnectionState = ConnectionState.Connected;
                    throw;
                }

                break;
        }
    }

    /// <summary>
    /// Sends the given command to the ATEM switcher
    /// </summary>
    public async Task SendCommandAsync(SerializedCommand command)
    {
        await _client.SendCommandAsync(command);
    }

    /// <summary>
    /// Sends the given commands to the ATEM switcher as a batch
    /// </summary>
    public async Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        await _client.SendCommandsAsync(commands);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (_receiveLoop != null) await _receiveLoop.Cancel();

        await _client.DisposeAsync();

        // Clean up any pending connection completion
        _connectionCompletionSource?.TrySetCanceled();
    }


    protected virtual void OnConnectionStateChanged(ConnectionState oldState, ConnectionState newState)
    {
        ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(oldState, newState));
    }
}
