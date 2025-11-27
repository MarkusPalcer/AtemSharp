using AtemSharp.Commands.Macro;
using AtemSharp.State.Macro;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class AtemSwitcherTests
{
    private AtemSwitcher _atem;
    private AtemClientFake _clientFake = new();
    private TestActionLoopFactory _actionLoopFactory;

    [SetUp]
    public void SetUp()
    {
        _clientFake = new();
        _actionLoopFactory = new TestActionLoopFactory();
        _atem = new("127.0.0.1", 1234, _clientFake, new LoggerFactory(), _actionLoopFactory);
    }

    [TearDown]
    public async Task TearDown()
    {
        // If connected: succeed disconnection
        if (_clientFake.RemoteEndPoint is not null)
        {
            _clientFake.SuccessfullyDisconnect();
        }

        await _atem.DisposeAsync();

        // Force stop all action loops
        _actionLoopFactory.Dispose();
    }

    [Test]
    public async Task ConnectAsync()
    {
        // Act
        var connectTask = _atem.ConnectAsync();
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));

        _clientFake.SuccessfullyConnect();
        await connectTask.WithTimeout();

        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Connected));

        // Assert
        Assert.That(_atem.State, Is.Not.Null);
    }

    [Test]
    public async Task ConnectAsync_WithCustomPort_ShouldUseSpecifiedPort()
    {
        // Arrange
        var customPort = 8888;

        // Act
        _atem = new("127.0.0.1", customPort, _clientFake, new LoggerFactory(), _actionLoopFactory);
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        // Assert
        Assert.That(_clientFake.RemoteEndPoint?.Port, Is.EqualTo(customPort));
    }

    [Test]
    public async Task ConnectAsync_WhileConnected_ShouldThrow()
    {
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _atem.ConnectAsync());
        Assert.That(ex!.Message, Does.Contain("Can not connect while"));
    }

    [Test]
    public void ConnectAsync_WhileConnecting_ShouldThrow()
    {
        _ = _atem.ConnectAsync();

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _atem.ConnectAsync().WithTimeout());
        Assert.That(ex!.Message, Does.Contain("Can not connect while"));
    }

    [Test]
    public void ConnectAsync_WithExceptionFromClient_ShouldThrow()
    {
        var exception = new Exception("Test");
        _clientFake.FailConnect(exception);

        var ex = Assert.ThrowsAsync<Exception>(async () => await _atem.ConnectAsync().WithTimeout());
        Assert.Multiple(() =>
        {
            Assert.That(ex, Is.SameAs(exception));
            Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
        });
    }

    [Test]
    public async Task DisconnectAsync()
    {
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        var disconnectTask = _atem.DisconnectAsync();
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnecting));
        _clientFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
    }

    [Test]
    public async Task DisconnectAsync_WithFailureFromClient_ShouldThrow()
    {
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        var exception = new Exception("Test");

        _clientFake.FailDisconnect(exception);
        var ex = Assert.ThrowsAsync<Exception>(async () => await _atem.DisconnectAsync().WithTimeout());
        Assert.That(ex, Is.SameAs(exception));
    }

    [Test]
    public void DisconnectAsync_WhenNotConnected_ShouldNotChangeAnything()
    {
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
        Assert.DoesNotThrowAsync(() => _atem.DisconnectAsync());
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
    }

    [Test]
    public void DisconnectAsync_WhileConnecting_ShouldThrow()
    {
        _ = _atem.ConnectAsync();
        Assert.ThrowsAsync<InvalidOperationException>(() => _atem.DisconnectAsync());
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));
    }

    [Test]
    public async Task DisconnectAsync_WhileDisconnecting_ShouldThrow()
    {
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        var disconnectTask = _atem.DisconnectAsync();
        Assert.That(disconnectTask.IsCompleted, Is.False);

        Assert.ThrowsAsync<InvalidOperationException>(() => _atem.DisconnectAsync());
    }

    [Test]
    public async Task ConnectAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        await _atem.DisposeAsync().AsTask();
        Assert.ThrowsAsync<ObjectDisposedException>(() => _atem.ConnectAsync());
    }

    [Test]
    public async Task ConnectAsync_StateInitialization_ShouldCreateNewState()
    {
        // Arrange
        var oldState = _atem.State;

        // Act
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        // Assert
        Assert.That(_atem.State, Is.TypeOf<AtemSharp.State.AtemState>());
        Assert.That(_atem.State, Is.Not.SameAs(oldState));
    }

    [Test]
    public async Task SendCommandAsync()
    {
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        var command = new MacroActionCommand(new Macro(), MacroAction.Run);
        await _atem.SendCommandAsync(command).WithTimeout();

        Assert.That(_clientFake.SentCommands, Is.EquivalentTo(new []{command}));
    }

    [Test]
    public void SendCommandAsync_WhileNotConnected_ShouldThrow()
    {
        var sendTask = _atem.SendCommandAsync(new MacroActionCommand(new Macro(), MacroAction.Run));
        Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
    }

    [Test]
    public async Task SendCommandsAsync()
    {
        _clientFake.SuccessfullyConnect();
        await _atem.ConnectAsync().WithTimeout();

        MacroActionCommand[] commands = [new(new Macro(), MacroAction.Run), new(new Macro(), MacroAction.Stop)];
        await _atem.SendCommandsAsync(commands).WithTimeout();

        Assert.That(_clientFake.SentCommands, Is.EquivalentTo(commands));
    }

    [Test]
    public void SendCommandsAsync_WhileNotConnected_ShouldThrow()
    {
        MacroActionCommand[] commands = [new(new Macro(), MacroAction.Run), new(new Macro(), MacroAction.Stop)];
        var sendTask = _atem.SendCommandsAsync(commands);
        Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
    }
}
