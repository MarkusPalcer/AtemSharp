using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class AtemSwitcherTests
{
    private AtemSwitcher _atem;
    private AtemClientFake _clientFake = new();

    [SetUp]
    public void SetUp()
    {
        _clientFake = new();
        _atem = new("127.0.0.1", 1234, _clientFake);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _atem.DisposeAsync();
        await _clientFake.DisposeAsync();
    }

    [Test]
    public async Task ConnectAsync_ShouldInitializeState()
    {
        // Act
        var connectTask = _atem.ConnectAsync();
        _clientFake.SuccessfullyConnect();
        await connectTask.WithTimeout();

        // Assert
        Assert.That(_atem.State, Is.Not.Null);

        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _clientFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task ConnectAsync_WithCustomPort_ShouldUseSpecifiedPort()
    {
        // Arrange
        var customPort = 8888;

        // Act
        _atem = new("127.0.0.1", customPort, _clientFake);
        var connectTask = _atem.ConnectAsync();
        _clientFake.SuccessfullyConnect();
        await connectTask.WithTimeout();

        // Assert
        Assert.That(_clientFake.RemoteEndPoint?.Port, Is.EqualTo(customPort));

        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _clientFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public Task DisconnectAsync_WhenNotConnected_ShouldNotThrow()
    {
        // Act & Assert
        Assert.DoesNotThrowAsync(() => _atem.DisconnectAsync());
        return Task.CompletedTask;
    }

    [Test]
    public async Task ConnectAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        // Arrange
        await _atem.DisposeAsync().AsTask();

        // Act & Assert
        Assert.ThrowsAsync<ObjectDisposedException>(() => _atem.ConnectAsync());
    }

    [Test]
    public async Task Dispose_ShouldDisposeTransport()
    {
        // Act
        await _atem.DisposeAsync().AsTask();

        // Assert
        Assert.That(_clientFake.IsDisposed, Is.True);
    }

    [Test]
    public async Task ConnectAsync_StateInitialization_ShouldCreateNewState()
    {
        // Arrange
        var oldState = _atem.State;

        // Act
        var connectTask = _atem.ConnectAsync();
        _clientFake.SuccessfullyConnect();
        await connectTask.WithTimeout();

        // Assert
        Assert.That(_atem.State, Is.TypeOf<State.AtemState>());
        Assert.That(_atem.State, Is.Not.SameAs(oldState));

        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _clientFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task MultipleConnectAttempts_ShouldHandleGracefully()
    {
        // Act & Assert - Multiple connect attempts should be handled properly
        var connectTask = _atem.ConnectAsync();
        _clientFake.SuccessfullyConnect();
        await connectTask.WithTimeout();

        // Second connect should fail since already connected
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _atem.ConnectAsync());
        Assert.That(ex!.Message, Does.Contain("Can not connect while"));

        // Cleanup
        var disconnectTask = _atem.DisconnectAsync();
        _clientFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
    }
}
