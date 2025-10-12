using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.Tests.TestUtilities;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AtemSharp.Tests;

[TestFixture]
[NonParallelizable]
public class AtemTests
{
    private Atem? _atem;
    private UdpTransportFake _transportFake;
    private ILogger<Atem> _logger;

    [SetUp]
    public void SetUp()
    {
        // Clear static state
        Atem.UnknownCommands.Clear();
        _logger = Substitute.For<ILogger<Atem>>();
        _atem = new Atem(_logger);
        _transportFake = new UdpTransportFake();
        _atem.Transport = _transportFake;
    }

    [TearDown]
    public void TearDown()
    {
        _atem?.Dispose();
        _transportFake.Dispose();
    }

    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.That(_atem!.State, Is.Null);
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Closed));
        Assert.That(Atem.UnknownCommands, Is.Not.Null);
        Assert.That(Atem.UnknownCommands, Is.Empty);
    }

    [Test]
    public async Task ConnectAsync_ShouldInitializeState()
    {
        // Act
        var connectTask = _atem!.ConnectAsync("127.0.0.1", 1234);
        _transportFake.SuccessfullyConnect();
        await connectTask.WithTimeout();

        // Assert
        Assert.That(_atem.State, Is.Not.Null);
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Established));

        
        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _transportFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task ConnectAsync_WithCustomPort_ShouldUseSpecifiedPort()
    {
        // Arrange
        var customPort = 8888;

        // Act
        var connectTask = _atem!.ConnectAsync("127.0.0.1", customPort);
        _transportFake.SuccessfullyConnect();
        await connectTask.WithTimeout();

        // Assert
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Established));
        Assert.That(_transportFake.RemoteEndPoint?.Port, Is.EqualTo(customPort));
        
        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _transportFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public Task DisconnectAsync_WhenNotConnected_ShouldNotThrow()
    {
	    // Act & Assert
        Assert.DoesNotThrowAsync(() => _atem!.DisconnectAsync());
        return Task.CompletedTask;
    }

    [Test]
    public async Task DisconnectAsync_AfterConnect_ShouldUpdateConnectionState()
    {
        // Arrange
        var connectTask = _atem!.ConnectAsync("127.0.0.1", 1234);
        _transportFake.SuccessfullyConnect();
        await connectTask.WithTimeout();
        
        // Act
        var disconnectTask = _atem.DisconnectAsync();
        _transportFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();

        // Assert
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Closed));
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
    public void Dispose_ShouldDisposeTransport()
    {
        // Act
        _atem!.Dispose();

        // Assert
        Assert.That(_transportFake.IsDisposed, Is.True);
    }

    [Test]
    public async Task ConnectAsync_StateInitialization_ShouldCreateNewState()
    {
        // Arrange
        Assert.That(_atem!.State, Is.Null);

        // Act
        var connectTask = _atem!.ConnectAsync("127.0.0.1", 1234);
        _transportFake.SuccessfullyConnect();
        await connectTask.WithTimeout();
        
        // Assert
        Assert.That(_atem.State, Is.Not.Null);
        Assert.That(_atem.State, Is.TypeOf<AtemSharp.State.AtemState>());

        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _transportFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task MultipleConnectAttempts_ShouldHandleGracefully()
    {
        // Act & Assert - Multiple connect attempts should be handled properly
        var connectTask = _atem!.ConnectAsync("127.0.0.1", 1234);
        _transportFake.SuccessfullyConnect();
        await connectTask.WithTimeout();
        
        // Second connect should fail since already connected
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _atem.ConnectAsync("127.0.0.1", 1234));
        Assert.That(ex!.Message, Does.Contain("Cannot connect when state is"));

        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _transportFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task ReceivedCommand_ShouldBeParsed()
    {
	    // Connect and wait for state initialization
	    var connectTask = _atem!.ConnectAsync("127.0.0.1", 1234);
	    _transportFake.SuccessfullyConnect();
	    await connectTask.WithTimeout();
	    
	    // Ensure state is initialized and set up capabilities for the test
	    Assert.That(_atem.State, Is.Not.Null, "State should be initialized after connection");
	    _atem.State!.Info.Capabilities = new AtemSharp.State.AtemCapabilities
	    {
		    MixEffects = 1  // Set up at least 1 mix effect
	    };

	    TaskCompletionSource? tcs = new TaskCompletionSource();
	    _logger.WhenForAnyArgs(x => x.Log(default, default, default, default, default, default))
	           .Do(_ => tcs?.SetResult());
	    
	    // Create a PreviewInputUpdateCommand packet manually (as sent by ATEM device)
	    // Command header (8 bytes): length (12), reserved (0), raw name "PrvI"
	    // Command data (4 bytes): mixEffectId=0, padding=0, source=0
	    var commandData = new byte[]
	    {
		    // Command header (8 bytes)
		    0x00, 0x0C, // Command length (12 bytes = 8 header + 4 data) - big endian
		    0x00, 0x00, // Reserved
		    (byte)'P', (byte)'r', (byte)'v', (byte)'I', // Raw name "PrvI"
            
		    // Command data (4 bytes)
		    0x00, // Mix effect ID = 0
		    0x00, // Padding byte
		    0x00, 0x00 // Source = 0 (big endian)
	    };
        
	    var packet = new AtemPacket(commandData)
	    {
		    Flags = PacketFlag.AckRequest,
		    SessionId = 1,
		    PacketId = 100
	    };
        
	    _transportFake.SimulatePacketReceived(packet);
	    await tcs.Task.WithTimeout();
	    
	    // Verify that no errors were logged (indicating successful command parsing)
	    _logger.DidNotReceive().Log(LogLevel.Error, Arg.Any<EventId>(), Arg.Any<object>(), Arg.Any<Exception>(), Arg.Any<Func<object, Exception?, string>>());
	    
	    // Also verify that the command was properly parsed and applied to state
	    Assert.That(_atem.State, Is.Not.Null, "State should be initialized after receiving commands");
	    Assert.That(_atem.State!.Video.MixEffects.ContainsKey(0), Is.True, "Mix effect 0 should exist after receiving PreviewInputUpdate command");
	    Assert.That(_atem.State.Video.MixEffects[0].PreviewInput, Is.EqualTo(0), "Preview input should be set to source 0");
	    
	    // Cleanup
	    tcs = null;
	    var disconnectTask = _atem.DisconnectAsync();
	    _transportFake.SuccessfullyDisconnect();
	    await disconnectTask.WithTimeout();
    }
}