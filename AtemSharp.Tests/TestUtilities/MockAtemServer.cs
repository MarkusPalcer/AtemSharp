using System.Net;
using System.Net.Sockets;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Tests.TestUtilities;

/// <summary>
/// A minimal UDP server that mimics ATEM device behavior for testing purposes.
/// This test double allows verification of data being sent and received.
/// </summary>
public class MockAtemServer : IDisposable
{
    private readonly UdpClient _udpServer;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly List<AtemPacket> _receivedPackets = new();
    private readonly List<AtemPacket> _sentPackets = new();
    private readonly object _lockObject = new();
    
    private Task? _serverTask;
    private ushort _sessionId;
    private ushort _nextPacketId = 1;
    private bool _disposed;

    /// <summary>
    /// Gets the port the mock server is listening on
    /// </summary>
    public int Port { get; private set; }

    /// <summary>
    /// Gets the local endpoint for the mock server
    /// </summary>
    public IPEndPoint LocalEndPoint => new(IPAddress.Loopback, Port);

    /// <summary>
    /// Creates a mock server instance that's ready to accept connections
    /// This is a convenience method that creates and starts the server in one call
    /// </summary>
    /// <returns>A started MockAtemServer instance</returns>
    public static MockAtemServer CreateAndStart()
    {
        var server = new MockAtemServer();
        server.Start();
        return server;
    }

    /// <summary>
    /// Gets a copy of all packets received by the server
    /// </summary>
    public IReadOnlyList<AtemPacket> ReceivedPackets
    {
        get
        {
            lock (_lockObject)
            {
                return _receivedPackets.ToList();
            }
        }
    }

    /// <summary>
    /// Gets a copy of all packets sent by the server
    /// </summary>
    public IReadOnlyList<AtemPacket> SentPackets
    {
        get
        {
            lock (_lockObject)
            {
                return _sentPackets.ToList();
            }
        }
    }

    /// <summary>
    /// Event raised when a packet is received
    /// </summary>
    public event EventHandler<AtemPacket>? PacketReceived;

    /// <summary>
    /// Event raised when a packet is sent
    /// </summary>
    public event EventHandler<AtemPacket>? PacketSent;

    /// <summary>
    /// Initializes a new instance of the MockAtemServer
    /// </summary>
    /// <param name="port">Port to listen on (0 for automatic assignment)</param>
    public MockAtemServer(int port = 0)
    {
        _udpServer = CreateUdpServerWithRetry(port);
        Port = ((IPEndPoint)_udpServer.Client.LocalEndPoint!).Port;
        _sessionId = (ushort)Random.Shared.Next(1, ushort.MaxValue);
    }

    /// <summary>
    /// Creates a UDP server with retry logic to handle port availability issues
    /// </summary>
    /// <param name="requestedPort">The requested port (0 for automatic assignment)</param>
    /// <returns>A bound UdpClient</returns>
    private static UdpClient CreateUdpServerWithRetry(int requestedPort)
    {
        const int maxRetries = 10;
        var retryCount = 0;

        while (retryCount < maxRetries)
        {
            try
            {
                // For concurrent test execution, always use automatic port assignment
                // to avoid conflicts between test instances
                var portToUse = requestedPort == 0 ? 0 : 0; // Force auto-assignment for test isolation
                return new UdpClient(portToUse);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
            {
                retryCount++;
                if (retryCount >= maxRetries)
                {
                    throw new InvalidOperationException(
                        $"Failed to bind to UDP port after {maxRetries} attempts. " +
                        "This may indicate port exhaustion or concurrent test execution issues.", ex);
                }
                
                // Brief delay before retry
                Thread.Sleep(10 * retryCount); // Exponential backoff
            }
        }

        throw new InvalidOperationException("Unexpected error in UDP server creation");
    }

    /// <summary>
    /// Starts the mock ATEM server
    /// </summary>
    public void Start()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MockAtemServer));

        _serverTask = ServerLoopAsync(_cancellationTokenSource.Token);
    }

    /// <summary>
    /// Stops the mock ATEM server
    /// </summary>
    public async Task StopAsync()
    {
        if (_disposed || _serverTask == null)
            return;

        _cancellationTokenSource.Cancel();
        
        try
        {
            await _serverTask.WaitAsync(TimeSpan.FromSeconds(5));
        }
        catch (OperationCanceledException)
        {
            // Expected when cancelling
        }
    }

    /// <summary>
    /// Clears all recorded packets
    /// </summary>
    public void ClearPacketHistory()
    {
        lock (_lockObject)
        {
            _receivedPackets.Clear();
            _sentPackets.Clear();
        }
    }

    /// <summary>
    /// Sends a custom packet to the connected client
    /// </summary>
    /// <param name="packet">Packet to send</param>
    /// <param name="remoteEndPoint">Client endpoint</param>
    public async Task SendPacketAsync(AtemPacket packet, IPEndPoint remoteEndPoint)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(MockAtemServer));

        packet.SessionId = _sessionId;
        packet.PacketId = _nextPacketId++;
        
        var packetData = packet.ToBytes();
        await _udpServer.SendAsync(packetData, remoteEndPoint);
        
        lock (_lockObject)
        {
            _sentPackets.Add(packet);
        }
        
        PacketSent?.Invoke(this, packet);
    }

    private async Task ServerLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var result = await _udpServer.ReceiveAsync(cancellationToken);
                    await HandleReceivedPacket(result.Buffer, result.RemoteEndPoint);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception)
                {
                    // Continue processing other packets even if one fails
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Expected when stopping
        }
    }

    private async Task HandleReceivedPacket(byte[] packetData, IPEndPoint clientEndPoint)
    {
        if (!AtemPacket.TryParse(packetData.AsSpan(), out var packet) || packet == null)
        {
            return;
        }

        lock (_lockObject)
        {
            _receivedPackets.Add(packet);
        }
        
        PacketReceived?.Invoke(this, packet);

        // Handle different packet types
        if (packet.HasFlag(PacketFlag.NewSessionId) && packet.HasFlag(PacketFlag.AckRequest))
        {
            // This is a hello packet - respond with hello response
            await HandleHelloPacket(packet, clientEndPoint);
        }
        else if (packet.HasFlag(PacketFlag.AckRequest))
        {
            // Send ACK for packets that request acknowledgment
            await SendAck(packet.PacketId, clientEndPoint);
        }
    }

    private async Task HandleHelloPacket(AtemPacket helloPacket, IPEndPoint clientEndPoint)
    {
        // Send back a response with NewSessionId flag to indicate connection established
        var response = new AtemPacket
        {
            Flags = PacketFlag.NewSessionId,
            SessionId = _sessionId,
            PacketId = _nextPacketId++,
            AckPacketId = helloPacket.PacketId,
            // Add minimal topology data payload to simulate real ATEM response
            Payload = CreateMinimalTopologyPayload()
        };

        await SendPacketAsync(response, clientEndPoint);
    }

    private async Task SendAck(ushort packetIdToAck, IPEndPoint clientEndPoint)
    {
        var ackPacket = AtemPacket.CreateAck(_sessionId, packetIdToAck);
        await SendPacketAsync(ackPacket, clientEndPoint);
    }

    private static byte[] CreateMinimalTopologyPayload()
    {
        // Create a minimal topology command to simulate device initialization
        // This is a simplified version that just indicates basic device information
        var payload = new List<byte>();
        
        // Add a simple "_top" (topology) command
        payload.AddRange([0x00, 0x0C]); // Command length (12 bytes)
        payload.AddRange("_top"u8.ToArray()); // Command name
        payload.AddRange([0x01, 0x00, 0x00, 0x00]); // Minimal topology data
        
        return payload.ToArray();
    }

    /// <summary>
    /// Gets the number of packets of a specific type that were received
    /// </summary>
    /// <param name="flag">Packet flag to count</param>
    /// <returns>Number of matching packets</returns>
    public int GetReceivedPacketCount(PacketFlag flag)
    {
        lock (_lockObject)
        {
            return _receivedPackets.Count(p => p.HasFlag(flag));
        }
    }

    /// <summary>
    /// Gets the number of packets of a specific type that were sent
    /// </summary>
    /// <param name="flag">Packet flag to count</param>
    /// <returns>Number of matching packets</returns>
    public int GetSentPacketCount(PacketFlag flag)
    {
        lock (_lockObject)
        {
            return _sentPackets.Count(p => p.HasFlag(flag));
        }
    }

    /// <summary>
    /// Waits for a specific number of packets with the given flag to be received
    /// </summary>
    /// <param name="flag">Packet flag to wait for</param>
    /// <param name="count">Number of packets to wait for</param>
    /// <param name="timeout">Maximum time to wait</param>
    /// <returns>True if the expected number of packets was received within the timeout</returns>
    public async Task<bool> WaitForReceivedPacketsAsync(PacketFlag flag, int count, TimeSpan timeout)
    {
        var startTime = DateTime.UtcNow;
        
        while (DateTime.UtcNow - startTime < timeout)
        {
            if (GetReceivedPacketCount(flag) >= count)
                return true;
                
            await Task.Delay(10);
        }
        
        return false;
    }

    public void Dispose()
    {
        if (_disposed)
            return;
            
        _disposed = true;
        
        _cancellationTokenSource.Cancel();
        
        if (_serverTask != null && !_serverTask.IsCompleted)
        {
            try
            {
                _serverTask.Wait(TimeSpan.FromSeconds(2));
            }
            catch
            {
                // Ignore timeout/cancellation exceptions during disposal
            }
        }
        
        _udpServer.Dispose();
        _cancellationTokenSource.Dispose();
        _serverTask?.Dispose();
    }
}