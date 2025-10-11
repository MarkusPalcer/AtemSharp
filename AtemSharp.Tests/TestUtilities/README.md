# MockAtemServer Test Utility

## Overview

The `MockAtemServer` is a minimal UDP server that mimics ATEM device behavior for testing purposes. This test double allows for realistic testing of the ATEM connection protocol while providing the ability to verify data being sent and received.

## Concurrent Test Execution

The `MockAtemServer` is designed to support concurrent test execution by automatically assigning available UDP ports. Each test instance gets its own port, preventing conflicts between parallel tests.

### Key Design Features:
- **Automatic Port Assignment**: Uses port 0 to let the OS assign available ports
- **Port Retry Logic**: Handles port availability issues with exponential backoff
- **Test Isolation**: Each test instance is completely independent

### Parallelization Options:

1. **Full Parallelization (Default)**: Tests can run concurrently
2. **Sequential Execution**: Add `[NonParallelizable]` if needed for debugging

```csharp
[TestFixture]
// [NonParallelizable] // Uncomment if you need sequential execution for debugging
public class MyAtemTests
{
    // Test implementation
}
```

## Features

- **Realistic Protocol Simulation**: Handles the ATEM hello handshake protocol
- **Packet Verification**: Tracks all sent and received packets for test assertions
- **Automatic Port Assignment**: Can use any available port for testing
- **Event-driven Architecture**: Provides events for packet received/sent notifications
- **Timeout Support**: Wait for specific packet conditions with timeouts

## Usage

### Basic Setup

```csharp
[TestFixture]
public class MyAtemTests
{
    private Atem? _atem;
    private MockAtemServer? _mockServer;

    [SetUp]
    public void SetUp()
    {
        _atem = new Atem();
        _mockServer = new MockAtemServer(); // Uses automatic port assignment
        _mockServer.Start();
    }

    [TearDown]
    public void TearDown()
    {
        _atem?.Dispose();
        _mockServer?.Dispose();
    }
}
```

### Connecting to the Mock Server

The key aspect is that the SuT (System Under Test) must use the dynamically assigned port from the mock server:

```csharp
[Test]
public async Task ConnectToMockServer()
{
    // Connect to the mock server using its dynamically assigned port
    var connectTask = _atem!.ConnectAsync("127.0.0.1", _mockServer!.Port);
    await Task.Delay(200); // Allow connection to establish
    
    // Verify connection
    Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Established));
    
    // Cleanup
    await _atem.DisconnectAsync();
    await connectTask;
}
```

### Port Assignment Strategy

The MockAtemServer uses these strategies to ensure concurrent test execution:

1. **Always uses port 0**: Forces OS to assign available ports
2. **Retry logic**: Handles rare port assignment failures  
3. **Per-test isolation**: Each test gets its own server instance
4. **Immediate availability**: Port is available immediately after construction

```csharp
// Each test gets a unique port automatically
using var server1 = new MockAtemServer(); // e.g., port 12345
using var server2 = new MockAtemServer(); // e.g., port 12346
using var server3 = new MockAtemServer(); // e.g., port 12347

// The SuT connects to the specific port
await atem.ConnectAsync("127.0.0.1", server1.Port);
```

### Verifying Packet Exchange

```csharp
[Test]
public async Task VerifyPacketExchange()
{
    // Clear any existing packet history
    _mockServer!.ClearPacketHistory();
    
    // Connect
    var connectTask = _atem!.ConnectAsync("127.0.0.1", _mockServer.Port);
    await Task.Delay(200);
    
    // Verify packets were exchanged
    Assert.That(_mockServer.GetReceivedPacketCount(PacketFlag.NewSessionId), Is.EqualTo(1));
    Assert.That(_mockServer.GetSentPacketCount(PacketFlag.NewSessionId), Is.EqualTo(1));
    
    // Get detailed packet information
    var receivedPackets = _mockServer.ReceivedPackets;
    var sentPackets = _mockServer.SentPackets;
    
    // Verify hello packet was received
    var helloPacket = receivedPackets.FirstOrDefault(p => p.HasFlag(PacketFlag.NewSessionId));
    Assert.That(helloPacket, Is.Not.Null);
    
    // Cleanup
    await _atem.DisconnectAsync();
    await connectTask;
}
```

### Waiting for Specific Packets

```csharp
[Test]
public async Task WaitForSpecificPackets()
{
    var connectTask = _atem!.ConnectAsync("127.0.0.1", _mockServer!.Port);
    
    // Wait for hello packet to be received (with timeout)
    var received = await _mockServer.WaitForReceivedPacketsAsync(
        PacketFlag.NewSessionId, 
        count: 1, 
        timeout: TimeSpan.FromSeconds(5)
    );
    
    Assert.That(received, Is.True);
    
    await _atem.DisconnectAsync();
    await connectTask;
}
```

### Custom Packet Handling

```csharp
[Test]
public async Task HandleCustomPackets()
{
    var receivedCustomPackets = new List<AtemPacket>();
    _mockServer!.PacketReceived += (_, packet) => 
    {
        if (packet.Payload.Length > 0)
            receivedCustomPackets.Add(packet);
    };
    
    // Connect and send custom packet
    var connectTask = _atem!.ConnectAsync("127.0.0.1", _mockServer.Port);
    await Task.Delay(200);
    
    // Send a custom packet through the transport
    var customPacket = new AtemPacket(new byte[] { 0x01, 0x02, 0x03 })
    {
        Flags = PacketFlag.AckRequest,
        PacketId = 100
    };
    
    // Note: This would require access to the transport layer
    // In practice, you'd send commands through the Atem class
    
    await _atem.DisconnectAsync();
    await connectTask;
}
```

## API Reference

### Constructor
- `MockAtemServer(int port = 0)` - Creates a mock server on the specified port (0 for automatic)

### Methods
- `Start()` - Starts the mock server
- `Task StopAsync()` - Stops the mock server
- `ClearPacketHistory()` - Clears all recorded packets
- `Task SendPacketAsync(AtemPacket packet, IPEndPoint remoteEndPoint)` - Sends a custom packet
- `int GetReceivedPacketCount(PacketFlag flag)` - Gets count of received packets with flag
- `int GetSentPacketCount(PacketFlag flag)` - Gets count of sent packets with flag
- `Task<bool> WaitForReceivedPacketsAsync(PacketFlag flag, int count, TimeSpan timeout)` - Waits for packets

### Properties
- `int Port` - The port the server is listening on
- `IReadOnlyList<AtemPacket> ReceivedPackets` - All packets received by the server
- `IReadOnlyList<AtemPacket> SentPackets` - All packets sent by the server

### Events
- `event EventHandler<AtemPacket>? PacketReceived` - Raised when a packet is received
- `event EventHandler<AtemPacket>? PacketSent` - Raised when a packet is sent

## Benefits of Using MockAtemServer

1. **Realistic Testing**: Tests actual UDP communication without needing real hardware
2. **Deterministic**: Controlled environment produces consistent test results
3. **Fast**: No network delays or connection timeouts
4. **Verifiable**: Can assert on exact packets sent and received
5. **Isolated**: Tests don't interfere with each other or external systems
6. **Debugging**: Easy to inspect packet flow for troubleshooting

## Differences from Real ATEM

The mock server implements only the minimal functionality needed for testing:

- Basic hello handshake protocol
- Automatic ACK responses for packets requesting acknowledgment
- Simple topology data response
- No advanced ATEM features (video routing, effects, etc.)

For comprehensive ATEM functionality testing, the mock server can be extended to handle additional command types and provide more realistic responses.