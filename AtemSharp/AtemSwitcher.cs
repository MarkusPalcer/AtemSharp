using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Communication;
using AtemSharp.DependencyInjection;
using AtemSharp.Lib;
using AtemSharp.State;

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
    private IActionLoop? _receiveLoop;
    private ConnectionState _connectionState = ConnectionState.Disconnected;
    private readonly IServices _services;

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

    public AtemSwitcher(string remoteHost, int remotePort, IServices services)
    {
        _services = services;
        _remoteHost = remoteHost;
        _remotePort = remotePort;

        _client = _services.CreateAtemClient();
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

        _receiveLoop = _services.StartActionLoop(ReceiveCommandLoop);

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
