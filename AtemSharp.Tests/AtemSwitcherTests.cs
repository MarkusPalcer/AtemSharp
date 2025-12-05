using AtemSharp.Commands;
using AtemSharp.Commands.Macro;
using AtemSharp.Lib;
using AtemSharp.State.Macro;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests;

[TestFixture]
[Parallelizable(ParallelScope.None)]
public class AtemSwitcherTests
{

    private AtemSwitcher _atem;
    private TestServices _services;

    [SetUp]
    public void SetUp()
    {
        _services = new TestServices();
        _atem = new AtemSwitcher("127.0.0.1", 1234, _services);
    }

    [TearDown]
    public async Task TearDown()
    {
        // If connected: succeed disconnection
        if (_services.ClientFake.RemoteEndPoint is not null)
        {
            _services.ClientFake.SuccessfullyDisconnect();
        }

        await _atem.DisposeAsync();

        // Force stop all action loops
        await _services.DisposeAsync();
    }

    [Test]
    public async Task ConnectAsync()
    {
        var connectTask = _atem.ConnectAsync();
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));

        _services.ClientFake.SuccessfullyConnect();
        await connectTask.TimesOut().WithTimeout();

        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await connectTask.WithTimeout();

        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Connected));
        Assert.That(_atem.State, Is.Not.Null);
    }

    [Test]
    public async Task ConnectAsync_WithCustomPort_ShouldUseSpecifiedPort()
    {
        // Arrange
        var customPort = 8888;

        // Act
        _atem = new("127.0.0.1", customPort, _services);
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        // Assert
        Assert.That(_services.ClientFake.RemoteEndPoint?.Port, Is.EqualTo(customPort));
    }

    [Test]
    public async Task ConnectAsync_WhileConnected_ShouldThrow()
    {
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
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
        _services.ClientFake.FailConnect(exception);

        var ex = Assert.ThrowsAsync<Exception>(async () => await _atem.ConnectAsync().WithTimeout());
        Assert.Multiple(() =>
        {
            Assert.That(ex, Is.SameAs(exception));
            Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
            Assert.That(_services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
        });
    }

    [Test]
    public async Task ConnectAsync_DisposeWhileWaitingForInitCommand()
    {
        var connectTask = _atem.ConnectAsync();
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));

        _services.ClientFake.SuccessfullyConnect();
        await connectTask.TimesOut().WithTimeout();

        await _atem.DisposeAsync().AsTask().WithTimeout();

        Assert.ThrowsAsync<TaskCanceledException>(async () => await connectTask.WithTimeout());

        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
        Assert.That(_services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
    }

    [Test]
    public async Task DisconnectAsync()
    {
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        var disconnectTask = _atem.DisconnectAsync();
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnecting));
        _services.ClientFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
            Assert.That(_services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
        });
    }

    [Test]
    public async Task DisconnectAsync_WithFailureFromClient_ShouldThrow()
    {
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        var exception = new Exception("Test");

        _services.ClientFake.FailDisconnect(exception);
        var ex = Assert.ThrowsAsync<Exception>(async () => await _atem.DisconnectAsync().WithTimeout());
        Assert.That(ex, Is.SameAs(exception));
    }

    [Test]
    public void DisconnectAsync_WhenNotConnected_ShouldNotChangeAnything()
    {
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
        Assert.DoesNotThrowAsync(() => _atem.DisconnectAsync().WithTimeout());
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
    }

    [Test]
    public void DisconnectAsync_WhileConnecting_ShouldThrow()
    {
        _ = _atem.ConnectAsync();
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _atem.DisconnectAsync().WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while transitioning connection states"));
        Assert.That(_atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));
    }

    [Test]
    public async Task DisconnectAsync_WhileDisconnecting_ShouldThrow()
    {
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        var disconnectTask = _atem.DisconnectAsync();
        Assert.That(disconnectTask.IsCompleted, Is.False);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _atem.DisconnectAsync().WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while transitioning connection states"));
    }

    [Test]
    public async Task ConnectAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        await _atem.DisposeAsync().AsTask();
        Assert.ThrowsAsync<ObjectDisposedException>(() => _atem.ConnectAsync().WithTimeout());
    }

    [Test]
    public async Task ConnectAsync_StateInitialization_ShouldCreateNewState()
    {
        // Arrange
        var oldState = _atem.State;

        // Act
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        // Assert
        Assert.That(_atem.State, Is.TypeOf<AtemSharp.State.AtemState>());
        Assert.That(_atem.State, Is.Not.SameAs(oldState));
    }

    [Test]
    public async Task SendCommandAsync()
    {
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        var command = new MacroActionCommand(new Macro(), MacroAction.Run);
        await _atem.SendCommandAsync(command).WithTimeout();

        Assert.That(_services.ClientFake.SentCommands, Is.EquivalentTo(new[] { command }));
    }

    [Test]
    public void SendCommandAsync_WhileNotConnected_ShouldThrow()
    {
        var sendTask = _atem.SendCommandAsync(new MacroActionCommand(new Macro(), MacroAction.Run));
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while not connected"));
    }

    [Test]
    public async Task SendCommandsAsync()
    {
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        MacroActionCommand[] commands = [new(new Macro(), MacroAction.Run), new(new Macro(), MacroAction.Stop)];
        await _atem.SendCommandsAsync(commands).WithTimeout();

        Assert.That(_services.ClientFake.SentCommands, Is.EquivalentTo(commands));
    }

    [Test]
    public void SendCommandsAsync_WhileNotConnected_ShouldThrow()
    {
        MacroActionCommand[] commands = [new(new Macro(), MacroAction.Run), new(new Macro(), MacroAction.Stop)];
        var sendTask = _atem.SendCommandsAsync(commands);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while not connected"));
    }

    [Test]
    public async Task DisposeAsync()
    {
        _services.ClientFake.SuccessfullyConnect();
        _services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await _atem.ConnectAsync().WithTimeout();

        _services.ClientFake.SuccessfullyDisconnect();
        await _atem.DisposeAsync().AsTask().WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(_services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
            Assert.That(_services.ClientFake.DisposeCounter, Is.EqualTo(1));
        });
    }
}
