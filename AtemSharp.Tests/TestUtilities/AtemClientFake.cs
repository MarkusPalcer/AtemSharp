using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Constants;
using AtemSharp.Enums;
using AtemSharp.Lib;
using JetBrains.Annotations;

namespace AtemSharp.Tests.TestUtilities;

/// <summary>
/// Fake implementation of IUdpTransport for testing purposes.
/// Provides control over connection state and allows tests to trigger events manually.
/// </summary>
public class AtemClientFake : IAtemClient
{
    private ConnectionState _connectionState = ConnectionState.Closed;
    private IPEndPoint? _remoteEndPoint;
    private bool _disposed;

    private TaskCompletionSource _connectTcs = new();
	private TaskCompletionSource _disconnectTcs = new();

	public bool IsDisposed => _disposed;

    /// <summary>
    /// Gets the remote endpoint if connected
    /// </summary>
    public IPEndPoint? RemoteEndPoint => _remoteEndPoint;

    /// <summary>
    /// List of packets that were sent via SendPacketAsync for verification in tests
    /// </summary>
    [UsedImplicitly]
    public List<SerializedCommand> SentCommands { get; } = new();

    public void SuccessfullyConnect()
    {
        _connectTcs.SetResult();

        // After successful connection, simulate the ATEM device sending an InitCompleteCommand
        // This is required for the new networking implementation to complete the connection
        SimulateReceivedCommand(new InitCompleteCommand());
    }

    public void FailConnect() => _connectTcs.SetException(new InvalidOperationException());

    public void SuccessfullyDisconnect() => _disconnectTcs.SetResult();
    public void FailDisconnect() => _disconnectTcs.SetException(new InvalidOperationException());

    /// <summary>
    /// Controls whether DisconnectAsync should succeed or fail
    /// </summary>
    public bool ShouldDisconnectSucceed { get; set; } = true;

    /// <summary>
    /// Exception to throw during connect if ShouldConnectSucceed is false
    /// </summary>
    public Exception? ConnectException { get; set; }

    /// <summary>
    /// Exception to throw during disconnect if ShouldDisconnectSucceed is false
    /// </summary>
    public Exception? DisconnectException { get; set; }

    public IReceivableSourceBlock<IDeserializedCommand> ReceivedCommands { get; } = new BufferBlock<IDeserializedCommand>();

    /// <summary>
    /// Simulates connecting to an ATEM device
    /// </summary>
    public async Task ConnectAsync(string address, int port = AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(AtemClientFake));
        }

        if (_connectionState != ConnectionState.Closed)
        {
            throw new InvalidOperationException($"Cannot connect when state is {_connectionState}");
        }

        _connectTcs = new();
        _disconnectTcs = new();

        await _connectTcs.Task;

        // Parse the IP address to validate it
        if (!IPAddress.TryParse(address, out var ipAddress))
        {
            throw new ArgumentException("Invalid IP address", nameof(address));
        }

        _remoteEndPoint = new IPEndPoint(ipAddress, port);
    }

    /// <summary>
    /// Simulates disconnecting from the ATEM device
    /// </summary>
    public async Task DisconnectAsync()
    {
        if (_disposed)
        {
            return; // Already disposed, nothing to do
        }

        if (_connectionState == ConnectionState.Closed)
        {
            return; // Already disconnected
        }

        await _disconnectTcs.Task;

        _remoteEndPoint = null;
    }

    public Task SendCommandAsync(SerializedCommand command)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(AtemClientFake));
        }

        if (_connectionState != ConnectionState.Established)
        {
            throw new InvalidOperationException($"Cannot send packet when state is {_connectionState}");
        }

        SentCommands.Add(command);
        return Task.CompletedTask;
    }

    public async Task SendCommandsAsync(IEnumerable<SerializedCommand> commands)
    {
        foreach (var command in commands)
        {
            await SendCommandAsync(command);
        }
    }

    public void SimulateReceivedCommand(IDeserializedCommand command)
    {
        ReceivedCommands.As<BufferBlock<IDeserializedCommand>>().SendAsync(command);
    }


    /// <summary>
    /// Disposes the fake transport and cleans up resources
    /// </summary>
    public ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return ValueTask.CompletedTask;
        }

        _disposed = true;

        // Clear sent packets
        SentCommands.Clear();

        // Reset state
        _connectionState = ConnectionState.Closed;
        _remoteEndPoint = null;

        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }
}
