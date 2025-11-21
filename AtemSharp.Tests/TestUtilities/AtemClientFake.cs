using System.Net;
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
    private ConnectionState _previousConnectionState = ConnectionState.Closed;
    private IPEndPoint? _remoteEndPoint;
    private bool _disposed;

    private TaskCompletionSource _connectTcs = new();
	private TaskCompletionSource _disconnectTcs = new();

	public bool IsDisposed => _disposed;

    /// <summary>
    /// Raised when a packet is received from the remote endpoint
    /// </summary>
    public event EventHandler<PacketReceivedEventArgs>? PacketReceived;

    /// <summary>
    /// Raised when the connection state changes
    /// </summary>
    public event EventHandler<ConnectionStateChangedEventArgs>? ConnectionStateChanged;

    /// <summary>
    /// Raised when an error occurs during transport operations
    /// </summary>
    public event EventHandler<Exception>? ErrorOccurred;

    /// <summary>
    /// Gets the current connection state
    /// </summary>
    public ConnectionState ConnectionState => _connectionState;

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
        SimulateInitCompleteCommand();
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

        // Simulate the connection state progression
        SetConnectionState(ConnectionState.SynSent);
        SetConnectionState(ConnectionState.Established);
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

        SetConnectionState(ConnectionState.Disconnected);
        _remoteEndPoint = null;
        SetConnectionState(ConnectionState.Closed);
    }

    public Task SendCommand(SerializedCommand command)
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

    /// <summary>
    /// Simulates receiving a packet from the ATEM device
    /// </summary>
    /// <param name="packet">The packet to simulate receiving</param>
    /// <param name="remoteEndPoint">The source endpoint (optional, uses current remote endpoint if null)</param>
    public void SimulatePacketReceived(AtemPacket packet, IPEndPoint? remoteEndPoint = null)
    {
        if (_disposed)
        {
            return;
        }

        if (packet == null)
        {
            throw new ArgumentNullException(nameof(packet));
        }

        var sourceEndPoint = remoteEndPoint ?? _remoteEndPoint ?? new IPEndPoint(IPAddress.Loopback, AtemConstants.DEFAULT_PORT);

        var eventArgs = new PacketReceivedEventArgs
        {
            Packet = packet
        };

        PacketReceived?.Invoke(this, eventArgs);
    }

    /// <summary>
    /// Simulates the ATEM device sending an InitCompleteCommand packet to signal
    /// that the connection initialization is complete
    /// </summary>
    public void SimulateInitCompleteCommand()
    {
        if (_disposed)
        {
            return;
        }

        // Create an InitCompleteCommand packet manually
        // Command header (8 bytes): length (8), reserved (0), raw name "InCm"
        // No command data for InitComplete - just the header
        var commandData = new byte[]
        {
            // Command header (8 bytes)
            0x00, 0x08, // Command length (8 bytes = header only) - big endian
            0x00, 0x00, // Reserved
            (byte)'I', (byte)'n', (byte)'C', (byte)'m', // Raw name "InCm"
            // No additional data for InitCompleteCommand
        };

        var packet = new AtemPacket(commandData)
        {
            Flags = PacketFlag.AckRequest,
            SessionId = 1,
            PacketId = 100
        };

        SimulatePacketReceived(packet);
    }

    /// <summary>
    /// Manually sets the connection state (for advanced test scenarios)
    /// </summary>
    /// <param name="newState">The new connection state</param>
    public void ForceConnectionState(ConnectionState newState)
    {
        if (_disposed)
        {
            return;
        }

        SetConnectionState(newState);
    }

    private void SetConnectionState(ConnectionState newState)
    {
        if (_connectionState == newState)
        {
            return;
        }

        _previousConnectionState = _connectionState;
        _connectionState = newState;

        var eventArgs = new ConnectionStateChangedEventArgs
        {
            State = newState,
            PreviousState = _previousConnectionState
        };

        ConnectionStateChanged?.Invoke(this, eventArgs);
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

        // Clear event handlers to prevent memory leaks
        PacketReceived = null;
        ConnectionStateChanged = null;
        ErrorOccurred = null;

        // Clear sent packets
        SentCommands.Clear();

        // Reset state
        _connectionState = ConnectionState.Closed;
        _remoteEndPoint = null;

        GC.SuppressFinalize(this);

        return ValueTask.CompletedTask;
    }
}
