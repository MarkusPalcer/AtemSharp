using AtemSharp.Enums;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests;

[TestFixture]
public class AtemTests
{
    private Atem? _atem;
    private MockAtemServer? _mockServer;

    [SetUp]
    public void SetUp()
    {
        // Clear static state
        Atem.UnknownCommands.Clear();
        _atem = new Atem();
        _mockServer = new MockAtemServer();
        _mockServer.Start();
    }

    [TearDown]
    public void TearDown()
    {
        _atem?.Dispose();
        _mockServer?.Dispose();
    }

    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.That(_atem!.State, Is.Null);
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Closed));
        Assert.That(_atem.CommandParser, Is.Not.Null);
        Assert.That(Atem.UnknownCommands, Is.Not.Null);
        Assert.That(Atem.UnknownCommands, Is.Empty);
    }

    [Test]
    public async Task ConnectAsync_ShouldInitializeState()
    {
        // Act
        var connectTask = _atem!.ConnectAsync("127.0.0.1", _mockServer!.Port);
        
        // Give some time for connection to establish
        await Task.Delay(300);

        // Assert
        Assert.That(_atem.State, Is.Not.Null);
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Established));

        // Verify the server received the hello packet
        Assert.That(_mockServer.GetReceivedPacketCount(PacketFlag.NewSessionId), Is.EqualTo(1));

        // Cleanup
        await _atem.DisconnectAsync();
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
        var connectTask = _atem!.ConnectAsync("127.0.0.1", customPort);
        await Task.Delay(300);

        // Assert
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Established));
        
        // Verify the custom server received the hello packet
        Assert.That(customMockServer.GetReceivedPacketCount(PacketFlag.NewSessionId), Is.EqualTo(1));

        // Cleanup
        await _atem.DisconnectAsync();
        await connectTask;
    }

    [Test]
    public void ConnectAsync_WithInvalidAddress_ShouldThrowArgumentException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() => _atem!.ConnectAsync("invalid-address"));
        Assert.That(ex!.Message, Does.Contain("Invalid IP address"));
    }

    [Test]
    public async Task DisconnectAsync_WhenNotConnected_ShouldNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrowAsync(() => _atem!.DisconnectAsync());
    }

    [Test]
    public async Task DisconnectAsync_AfterConnect_ShouldUpdateConnectionState()
    {
        // Arrange
        var connectTask = _atem!.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(300); // Allow connection to establish

        // Act
        await _atem.DisconnectAsync();

        // Assert
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Closed));

        // Cleanup
        await connectTask;
    }

    [Test]
    public void ConnectAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        _atem!.Dispose();

        // Act & Assert
        Assert.ThrowsAsync<ObjectDisposedException>(() => _atem.ConnectAsync("127.0.0.1"));
    }

    [Test]
    public void Dispose_ShouldBeIdempotent()
    {
        // Act & Assert
        Assert.DoesNotThrow(() => _atem!.Dispose());
        Assert.DoesNotThrow(() => _atem!.Dispose());
    }

    [Test]
    public void Dispose_ShouldUpdateConnectionState()
    {
        // Act
        _atem!.Dispose();

        // Assert
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Closed));
    }

    [Test]
    public async Task ConnectAsync_StateInitialization_ShouldCreateNewState()
    {
        // Arrange
        Assert.That(_atem!.State, Is.Null);

        // Act
        var connectTask = _atem.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(300); // Allow connection to establish

        // Assert
        Assert.That(_atem.State, Is.Not.Null);
        Assert.That(_atem.State, Is.TypeOf<AtemSharp.State.AtemState>());

        // Cleanup
        await _atem.DisconnectAsync();
        await connectTask;
    }

    [Test]
    public void UnknownCommands_ShouldBeStaticAndShared()
    {
        // Arrange
        using var atem1 = new Atem();
        using var atem2 = new Atem();

        // Act
        Atem.UnknownCommands.Add("TEST1");

        // Assert
        Assert.That(Atem.UnknownCommands, Contains.Item("TEST1"));
        Assert.That(Atem.UnknownCommands.Count, Is.EqualTo(1));
        
        // Both instances should see the same static collection
        Assert.That(Atem.UnknownCommands, Is.SameAs(Atem.UnknownCommands));
    }

    [Test]
    public void CommandParser_ShouldBeAccessible()
    {
        // Assert
        Assert.That(_atem!.CommandParser, Is.Not.Null);
        Assert.That(_atem.CommandParser, Is.TypeOf<AtemSharp.Lib.CommandParser>());
    }

    [Test]
    public async Task MultipleConnectAttempts_ShouldHandleGracefully()
    {
        // Act & Assert - Multiple connect attempts should be handled properly
        var connectTask1 = _atem!.ConnectAsync("127.0.0.1", _mockServer!.Port);
        await Task.Delay(50);

        // Second connect should fail since already connecting
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _atem.ConnectAsync("127.0.0.1", _mockServer.Port));
        Assert.That(ex!.Message, Does.Contain("Cannot connect when state is"));

        // Cleanup
        await _atem.DisconnectAsync();
        await connectTask1;
    }

    [Test]
    public async Task Connection_ShouldExchangePacketsWithMockServer()
    {
        // Arrange
        _mockServer!.ClearPacketHistory();

        // Act
        var connectTask = _atem!.ConnectAsync("127.0.0.1", _mockServer.Port);
        await Task.Delay(300); // Allow full connection handshake

        // Assert connection was established
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Established));

        // Verify packet exchange with mock server
        var receivedPackets = _mockServer.ReceivedPackets;
        var sentPackets = _mockServer.SentPackets;

        // Should have received hello packet from Atem
        Assert.That(receivedPackets.Count, Is.GreaterThan(0));
        var helloPacket = receivedPackets.FirstOrDefault(p => p.HasFlag(PacketFlag.NewSessionId) && p.HasFlag(PacketFlag.AckRequest));
        Assert.That(helloPacket, Is.Not.Null, "Should have received hello packet from Atem");

        // Should have sent hello response back to Atem
        Assert.That(sentPackets.Count, Is.GreaterThan(0));
        var helloResponse = sentPackets.FirstOrDefault(p => p.HasFlag(PacketFlag.NewSessionId));
        Assert.That(helloResponse, Is.Not.Null, "Should have sent hello response to Atem");

        // Verify hello response contains topology data
        Assert.That(helloResponse!.Payload.Length, Is.GreaterThan(0), "Hello response should contain topology data");

        // Cleanup
        await _atem.DisconnectAsync();
        await connectTask;
    }
}