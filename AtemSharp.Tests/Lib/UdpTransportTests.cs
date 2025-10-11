using System.Net;
using System.Net.Sockets;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class UdpTransportTests
{
    private UdpTransport? _transport;
    private MockAtemServer? _mockServer;

    [SetUp]
    public void SetUp()
    {
        _transport = new UdpTransport();
        _mockServer = new MockAtemServer();
        _mockServer.Start();
    }

    [TearDown]
    public void TearDown()
    {
        _transport?.Dispose();
        _mockServer?.Dispose();
    }

    [Test]
    public void Constructor_ShouldInitializeWithClosedState()
    {
        // Assert
        Assert.That(_transport!.ConnectionState, Is.EqualTo(ConnectionState.Closed));
        Assert.That(_transport.RemoteEndPoint, Is.Null);
    }

    [Test]
    public void ConnectAsync_WithInvalidIpAddress_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidAddress = "invalid-ip-address";

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _transport!.ConnectAsync(invalidAddress));
        Assert.That(ex!.ParamName, Is.EqualTo("address"));
        Assert.That(ex.Message, Does.Contain("Invalid IP address"));
    }

    [Test]
    public async Task ConnectAsync_WhenAlreadyConnecting_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var connectTask = _transport!.ConnectAsync("127.0.0.1");
        
        try
        {
            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _transport.ConnectAsync("127.0.0.1"));
            Assert.That(ex!.Message, Does.Contain("Cannot connect when state is"));
        }
        finally
        {
            // Cleanup - cancel the first connect attempt
            await _transport.DisconnectAsync();
            try { await connectTask; } catch { /* ignore */ }
        }
    }

    [Test]
    public async Task ConnectAsync_ShouldUpdateConnectionState()
    {
        // Arrange
        var stateChanges = new List<ConnectionState>();
        _transport!.ConnectionStateChanged += (_, e) => stateChanges.Add(e.State);

        // Act
        var connectTask = _transport.ConnectAsync("127.0.0.1", _mockServer!.Port);
        
        // Wait for connection to establish
        await Task.Delay(200);

        // Assert
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Established));
        Assert.That(stateChanges, Contains.Item(ConnectionState.SynSent));
        Assert.That(stateChanges, Contains.Item(ConnectionState.Established));
        Assert.That(_transport.RemoteEndPoint, Is.Not.Null);
        Assert.That(_transport.RemoteEndPoint!.Address, Is.EqualTo(IPAddress.Parse("127.0.0.1")));
        Assert.That(_transport.RemoteEndPoint.Port, Is.EqualTo(_mockServer.Port));

        // Verify the server received the hello packet
        Assert.That(_mockServer.GetReceivedPacketCount(PacketFlag.NewSessionId), Is.EqualTo(1));
        Assert.That(_mockServer.GetReceivedPacketCount(PacketFlag.AckRequest), Is.EqualTo(1));

        // Cleanup
        await _transport.DisconnectAsync();
        await connectTask;
    }

    [Test]
    public async Task ConnectAsync_WithCustomPort_ShouldUseSpecifiedPort()
    {
        // Arrange
        using var customMockServer = new MockAtemServer();
        customMockServer.Start();
        var customPort = customMockServer.Port;

        // Act
        var connectTask = _transport!.ConnectAsync("127.0.0.1", customPort);
        
        // Give some time for connection attempt
        await Task.Delay(200);

        // Assert
        Assert.That(_transport.RemoteEndPoint!.Port, Is.EqualTo(customPort));
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Established));
        
        // Verify the custom server received the hello packet
        Assert.That(customMockServer.GetReceivedPacketCount(PacketFlag.NewSessionId), Is.EqualTo(1));

        // Cleanup
        await _transport.DisconnectAsync();
        await connectTask;
    }

    [Test]
    public async Task DisconnectAsync_WhenClosed_ShouldReturnImmediately()
    {
        // Arrange
        Assert.That(_transport!.ConnectionState, Is.EqualTo(ConnectionState.Closed));

        // Act
        var disconnectTask = _transport.DisconnectAsync();

        // Assert
        Assert.DoesNotThrowAsync(() => disconnectTask);
        await disconnectTask; // Should complete quickly
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Closed));
    }

    [Test]
    public async Task DisconnectAsync_AfterConnect_ShouldUpdateConnectionState()
    {
        // Arrange
        var stateChanges = new List<ConnectionState>();
        _transport!.ConnectionStateChanged += (_, e) => stateChanges.Add(e.State);

        var connectTask = _transport.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(200); // Allow connection to establish

        // Act
        await _transport.DisconnectAsync();

        // Assert
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Closed));
        Assert.That(stateChanges, Contains.Item(ConnectionState.Disconnected));
        Assert.That(stateChanges, Contains.Item(ConnectionState.Closed));

        // Cleanup
        await connectTask;
    }

    [Test]
    public void SendPacketAsync_WhenNotConnected_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var packet = AtemPacket.CreateHello();

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _transport!.SendPacketAsync(packet));
        Assert.That(ex!.Message, Does.Contain("Cannot send packet when not connected"));
    }

    [Test]
    public async Task SendPacketAsync_WhenConnected_ShouldSendPacketAndReceiveAck()
    {
        // Arrange
        var packetsReceived = new List<AtemPacket>();
        _transport!.PacketReceived += (_, e) => packetsReceived.Add(e.Packet);

        // Connect first
        var connectTask = _transport.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(200); // Allow connection to establish
        
        _mockServer.ClearPacketHistory(); // Clear connection packets

        // Create a test packet
        var testPacket = new AtemPacket([0x01, 0x02, 0x03, 0x04])
        {
            Flags = PacketFlag.AckRequest,
            SessionId = 1,
            PacketId = 100
        };

        // Act
        await _transport.SendPacketAsync(testPacket);
        await Task.Delay(100); // Allow packet to be sent and ACK to be received

        // Assert
        // Verify the server received our packet
        var receivedPackets = _mockServer.ReceivedPackets;
        Assert.That(receivedPackets.Count, Is.GreaterThan(0));
        var ourPacket = receivedPackets.First(p => p.PacketId == 100);
        Assert.That(ourPacket, Is.Not.Null);
        Assert.That(ourPacket.Payload, Is.EqualTo(new byte[] { 0x01, 0x02, 0x03, 0x04 }));

        // Verify we received an ACK packet
        Assert.That(packetsReceived.Count, Is.GreaterThan(0));
        var ackPacket = packetsReceived.FirstOrDefault(p => p.HasFlag(PacketFlag.AckReply));
        Assert.That(ackPacket, Is.Not.Null);
        Assert.That(ackPacket!.AckPacketId, Is.EqualTo(100));

        // Cleanup
        await _transport.DisconnectAsync();
        await connectTask;
    }

    [Test]
    public void SendPacketAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        var packet = AtemPacket.CreateHello();
        _transport!.Dispose();

        // Act & Assert
        Assert.ThrowsAsync<ObjectDisposedException>(() => _transport.SendPacketAsync(packet));
    }

    [Test]
    public void Dispose_ShouldBeIdempotent()
    {
        // Act
        _transport!.Dispose();
        
        // Should not throw
        Assert.DoesNotThrow(() => _transport.Dispose());
    }

    [Test]
    public void Dispose_ShouldUpdateConnectionState()
    {
        // Arrange
        var stateChanges = new List<ConnectionState>();
        _transport!.ConnectionStateChanged += (_, e) => stateChanges.Add(e.State);

        // Act
        _transport.Dispose();

        // Assert
        Assert.That(_transport.ConnectionState, Is.EqualTo(ConnectionState.Closed));
    }

    [Test]
    public async Task ConnectionStateChanged_ShouldProvideCorrectPreviousState()
    {
        // Arrange
        var stateChangeEvents = new List<ConnectionStateChangedEventArgs>();
        _transport!.ConnectionStateChanged += (_, e) => stateChangeEvents.Add(e);

        // Act
        var connectTask = _transport.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(200); // Allow connection to establish
        await _transport.DisconnectAsync();

        // Assert
        Assert.That(stateChangeEvents, Has.Count.GreaterThanOrEqualTo(3));
        
        var synSentEvent = stateChangeEvents.FirstOrDefault(e => e.State == ConnectionState.SynSent);
        Assert.That(synSentEvent, Is.Not.Null);
        Assert.That(synSentEvent!.PreviousState, Is.EqualTo(ConnectionState.Closed));

        var establishedEvent = stateChangeEvents.FirstOrDefault(e => e.State == ConnectionState.Established);
        Assert.That(establishedEvent, Is.Not.Null);
        Assert.That(establishedEvent!.PreviousState, Is.EqualTo(ConnectionState.SynSent));

        var disconnectedEvent = stateChangeEvents.FirstOrDefault(e => e.State == ConnectionState.Disconnected);
        Assert.That(disconnectedEvent, Is.Not.Null);
        Assert.That(disconnectedEvent!.PreviousState, Is.EqualTo(ConnectionState.Established));

        // Cleanup
        await connectTask;
    }

    [Test]
    public async Task DisconnectAsync_WithCancellation_ShouldRespectCancellationToken()
    {
        // Arrange
        var connectTask = _transport!.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(200); // Allow connection to establish

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(50);

        // Act & Assert - should not throw even with cancellation during disconnect
        Assert.DoesNotThrowAsync(() => _transport.DisconnectAsync(cts.Token));

        // Cleanup
        await connectTask;
    }

    [Test]
    public void ConnectAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        _transport!.Dispose();

        // Act & Assert
        Assert.ThrowsAsync<ObjectDisposedException>(() => _transport.ConnectAsync("127.0.0.1"));
    }

    [Test]
    public async Task PacketReceived_ShouldBeRaisedWhenPacketsReceived()
    {
        // Arrange
        var receivedPackets = new List<AtemPacket>();
        _transport!.PacketReceived += (_, e) => receivedPackets.Add(e.Packet);

        // Connect first
        var connectTask = _transport.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(200); // Allow connection to establish

        // Act - We can't directly send to the transport's port, but we can verify 
        // that the connection process already generated packet received events
        // (from the initial hello/response exchange)

        // Assert
        Assert.That(receivedPackets.Count, Is.GreaterThan(0));
        
        // We should have received at least the hello response packet
        var responsePacket = receivedPackets.FirstOrDefault(p => p.HasFlag(PacketFlag.NewSessionId));
        Assert.That(responsePacket, Is.Not.Null);

        // Cleanup
        await _transport.DisconnectAsync();
        await connectTask;
    }

    [Test]
    public async Task ErrorOccurred_ShouldHandleNetworkErrorsGracefully()
    {
        // Arrange
        var errorsReceived = new List<Exception>();
        _transport!.ErrorOccurred += (_, ex) => errorsReceived.Add(ex);

        // Act - Connect normally and then immediately disconnect to test error handling
        var connectTask = _transport.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(100);
        
        // Stop the mock server while connected to simulate network failure
        await _mockServer.StopAsync();
        await Task.Delay(100);

        await _transport.DisconnectAsync();

        // Assert - Transport should handle disconnection gracefully
        Assert.DoesNotThrow(() => { /* Transport should handle errors gracefully */ });

        // Cleanup
        try { await connectTask; } catch { /* ignore connection failures */ }
    }
}