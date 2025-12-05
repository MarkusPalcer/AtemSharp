using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.FrameworkAbstraction;
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
    private TaskCompletionSource? _connectionCompletionSource;
    private ActionLoop? _receiveLoop;
    private ConnectionState _connectionState = ConnectionState.Disconnected;
    private readonly ILogger<AtemSwitcher> _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IActionLoopFactory _actionLoopFactory;

    /// <inheritdoc />
    public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

    /// <inheritdoc />
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

    /// <inheritdoc />
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
    [ExcludeFromCodeCoverage]
    public AtemSwitcher(string remoteHost, int remotePort = Constants.AtemConstants.DefaultPort, ILoggerFactory? loggerFactory = null)
        : this(remoteHost, remotePort, null, loggerFactory, null)
    {
    }

    // internal constructor for passing in mocked AtemClient during tests
    internal AtemSwitcher(string remoteHost, int remotePort, IAtemClient? transport, ILoggerFactory? loggerFactory, IActionLoopFactory? actionLoopFactory)
    {
        _remoteHost = remoteHost;
        _remotePort = remotePort;
        loggerFactory ??= new DebugLoggerFactory();
        _actionLoopFactory = actionLoopFactory ?? new ActionLoop.Factory();
        _client = transport ?? new AtemClient(loggerFactory, ProtocolFactory, _actionLoopFactory);
        _logger =  loggerFactory.CreateLogger<AtemSwitcher>();
        _loggerFactory = loggerFactory;
    }

    [ExcludeFromCodeCoverage]
    private AtemProtocol ProtocolFactory()
    {
        return new AtemProtocol(_loggerFactory.CreateLogger<AtemProtocol>(),
                                () => new UdpClientWrapper(),
                                new SystemTimeProvider(), _actionLoopFactory);
    }

    /// <inheritdoc />
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (ConnectionState != ConnectionState.Disconnected)
        {
            throw new InvalidOperationException("Can not connect while not disconnected");
        }

        ConnectionState = ConnectionState.Connecting;

        State = new AtemState();
        _connectionCompletionSource = new TaskCompletionSource();

        try
        {
            await _client.ConnectAsync(_remoteHost, _remotePort);
        }
        catch (Exception)
        {
            ConnectionState = ConnectionState.Disconnected;
            throw;
        }

        _receiveLoop = _actionLoopFactory.Start(ReceiveCommandLoop, _logger);

        try {
            // Wait for InitCompleteCommand to be received, indicating the connection is fully established
           await _connectionCompletionSource.Task.WaitAsync(cancellationToken);
        }
        catch (Exception)
        {
            ConnectionState = ConnectionState.Disconnected;
            await StopReceiveLoop();
            throw;
        }

        ConnectionState = ConnectionState.Connected;
    }

    private async Task StopReceiveLoop()
    {
        if (_receiveLoop != null)
        {
            await _receiveLoop.Cancel();
            _receiveLoop = null;
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
            _connectionCompletionSource?.TrySetResult();
        }
    }

    /// <inheritdoc />
    public async Task DisconnectAsync()
    {
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
                    await StopReceiveLoop();

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

    /// <inheritdoc />
    public async Task SendCommandAsync(SerializedCommand command)
    {
        if (ConnectionState != ConnectionState.Connected)
        {
            throw new InvalidOperationException("Can not send commands while not connected");
        }

        await _client.SendCommandAsync(command);
    }

    /// <inheritdoc />
    public async Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        if (ConnectionState != ConnectionState.Connected)
        {
            throw new InvalidOperationException("Can not send commands while not connected");
        }

        await _client.SendCommandsAsync(commands);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        _disposed = true;
        _connectionCompletionSource?.TrySetCanceled();
        await _client.DisposeAsync();
        await StopReceiveLoop();
    }

    private void OnConnectionStateChanged(ConnectionState oldState, ConnectionState newState)
    {
        ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(oldState, newState));
    }
}
