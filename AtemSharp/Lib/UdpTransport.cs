using System.Net;
using System.Net.Sockets;
using AtemSharp.Constants;
using AtemSharp.Enums;

namespace AtemSharp.Lib;

/// <summary>
/// Event arguments for packet received events
/// </summary>
public class PacketReceivedEventArgs : EventArgs
{
    public required AtemPacket Packet { get; init; }
    public required IPEndPoint RemoteEndPoint { get; init; }
}

/// <summary>
/// Event arguments for connection state change events
/// </summary>
public class ConnectionStateChangedEventArgs : EventArgs
{
    public required ConnectionState State { get; init; }
    public required ConnectionState PreviousState { get; init; }
}

/// <summary>
/// UDP transport layer for ATEM protocol communication.
/// Handles connection state management, packet reliability, and async message processing.
/// </summary>
public sealed class UdpTransport : IUdpTransport
{
    private readonly IUdpClient _udpClient;
    private readonly object _lockObject = new();
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly SemaphoreSlim _sendSemaphore = new(1, 1);
    
    private ConnectionState _connectionState = ConnectionState.Closed;
    private IPEndPoint? _remoteEndPoint;
    private Task? _receiveTask;
    private bool _disposed;

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
    public ConnectionState ConnectionState
    {
        get
        {
            lock (_lockObject)
            {
                return _connectionState;
            }
        }
        private set
        {
            ConnectionState previousState;
            lock (_lockObject)
            {
                previousState = _connectionState;
                _connectionState = value;
            }
            
            if (previousState != value)
            {
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs 
                { 
                    State = value, 
                    PreviousState = previousState 
                });
            }
        }
    }

    /// <summary>
    /// Gets the remote endpoint if connected
    /// </summary>
    public IPEndPoint? RemoteEndPoint => _remoteEndPoint;

    /// <summary>
    /// Initializes a new instance of the UdpTransport class
    /// </summary>
    public UdpTransport() : this(new UdpClientWrapper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the UdpTransport class with the specified UDP client
    /// This constructor is useful for testing scenarios where you want to inject a mock UDP client
    /// </summary>
    /// <param name="udpClient">The UDP client to use for communication</param>
    public UdpTransport(IUdpClient udpClient)
    {
        _udpClient = udpClient ?? throw new ArgumentNullException(nameof(udpClient));
        _udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, 0)); // Bind to any available port
    }

    /// <summary>
    /// Connects to the specified ATEM device
    /// </summary>
    /// <param name="address">IP address of the ATEM device</param>
    /// <param name="port">Port number (default: 9910)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task that completes when connected</returns>
    public async Task ConnectAsync(string address, int port = AtemConstants.DEFAULT_PORT, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        
        if (ConnectionState != ConnectionState.Closed)
        {
            throw new InvalidOperationException($"Cannot connect when state is {ConnectionState}");
        }

        if (!IPAddress.TryParse(address, out var ipAddress))
        {
            throw new ArgumentException($"Invalid IP address: {address}", nameof(address));
        }
        _remoteEndPoint = new IPEndPoint(ipAddress, port);

        try
        {
            // Connect the UDP client to the remote endpoint
            _udpClient.Connect(_remoteEndPoint);
            
            // Start the receive loop
            _receiveTask = ReceiveLoopAsync(_cancellationTokenSource.Token);
            
            ConnectionState = ConnectionState.SendingSyn;
            
            // Send the initial ATEM hello packet directly (no packet wrapper)
            // This matches the TypeScript implementation which sends COMMAND_CONNECT_HELLO raw
            await _udpClient.SendAsync(AtemConstants.HELLO_PACKET, cancellationToken);
			
            ConnectionState = ConnectionState.SynSent;
            
            // The connection state will be set to Established when we receive the proper response
            // For now, we remain in SynSent state until the ATEM protocol handshake completes
        }
        catch (Exception ex)
        {
            ConnectionState = ConnectionState.Closed;
            _remoteEndPoint = null;
            throw new InvalidOperationException($"Failed to connect to {address}:{port}", ex);
        }
    }

    /// <summary>
    /// Disconnects from the ATEM device
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task that completes when disconnected</returns>
    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (ConnectionState == ConnectionState.Closed)
        {
            return;
        }

        ConnectionState = ConnectionState.Disconnected;

        try
        {
            // Cancel the receive loop
            await _cancellationTokenSource.CancelAsync();
            
            // Wait for receive task to complete (with timeout)
            if (_receiveTask != null)
            {
                using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                timeoutCts.CancelAfter(TimeSpan.FromSeconds(5));
                
                try
                {
                    await _receiveTask.WaitAsync(timeoutCts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Timeout or external cancellation - this is expected
                }
            }
        }
        finally
        {
            ConnectionState = ConnectionState.Closed;
            _remoteEndPoint = null;
        }
    }

    /// <summary>
    /// Sends a packet to the connected ATEM device
    /// </summary>
    /// <param name="packet">Packet to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task that completes when packet is sent</returns>
    public async Task SendPacketAsync(AtemPacket packet, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
        
        if (ConnectionState == ConnectionState.Closed)
        {
            throw new InvalidOperationException("Cannot send packet when not connected");
        }

        if (_remoteEndPoint == null)
        {
            throw new InvalidOperationException("Remote endpoint not set");
        }

        // Serialize the packet
        var packetData = packet.ToBytes();
        
        // Use semaphore to ensure thread-safe sending
        await _sendSemaphore.WaitAsync(cancellationToken);
        try
        {
            await _udpClient.SendAsync(packetData, cancellationToken);
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
            throw;
        }
        finally
        {
            _sendSemaphore.Release();
        }
    }

    /// <summary>
    /// Sends the initial hello packet to establish connection
    /// </summary>
    public async Task SendHelloPacketAsync(CancellationToken cancellationToken)
    {
        var helloPacket = AtemPacket.CreateHello();
        await SendPacketAsync(helloPacket, cancellationToken);
    }

    /// <summary>
    /// Main receive loop that processes incoming packets
    /// </summary>
    private async Task ReceiveLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await _udpClient.ReceiveAsync(cancellationToken);

                    if (cancellationToken.IsCancellationRequested) return;
                    
                    // Parse the received packet
                    if (AtemPacket.TryParse(result.Buffer.AsSpan(), out var packet) && packet != null)
                    {
	                    if (cancellationToken.IsCancellationRequested) return;
                        // Update connection state based on received packet
                        UpdateConnectionStateFromPacket(packet);
                        
                        // Raise packet received event
                        PacketReceived?.Invoke(this, new PacketReceivedEventArgs 
                        { 
                            Packet = packet, 
                            RemoteEndPoint = result.RemoteEndPoint 
                        });
                    }
                    else
                    {
	                    if (cancellationToken.IsCancellationRequested) return;
                        // Invalid packet received - log but continue
                        ErrorOccurred?.Invoke(this, new InvalidDataException("Received invalid packet data"));
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                    break;
                }
                catch (ObjectDisposedException)
                {
                    // Expected when disposing
                    break;
                }
                catch (Exception ex)
                {
                    ErrorOccurred?.Invoke(this, ex);
                    
                    // For serious errors, break the loop
                    if (ex is SocketException)
                    {
                        break;
                    }
                }
            }
        }
        finally
        {
            // Ensure we're marked as disconnected when receive loop exits
            if (ConnectionState != ConnectionState.Closed)
            {
                ConnectionState = ConnectionState.Closed;
            }
        }
    }

    /// <summary>
    /// Updates connection state based on received packet flags
    /// </summary>
    private void UpdateConnectionStateFromPacket(AtemPacket packet)
    {
        if (ConnectionState == ConnectionState.SynSent && 
            packet.Flags.HasFlag(PacketFlag.NewSessionId))
        {
            // Received response to hello packet - connection established
            ConnectionState = ConnectionState.Established;
        }
    }

    /// <summary>
    /// Throws ObjectDisposedException if the transport has been disposed
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(UdpTransport));
        }
    }

    /// <summary>
    /// Disposes the UDP transport and releases all resources
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        try
        {
            // Cancel operations and disconnect
            _cancellationTokenSource.Cancel();
        }
        finally
        {
            // Dispose resources
            _udpClient.Dispose();
            _cancellationTokenSource.Dispose();
            _sendSemaphore.Dispose();
        }
    }
}