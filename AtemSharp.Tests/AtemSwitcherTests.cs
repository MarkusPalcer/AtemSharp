using AtemSharp.Commands;
using AtemSharp.Commands.Macro;
using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.Lib;
using AtemSharp.State.Macro;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests;

[TestFixture]
public class AtemSwitcherTests
{

    private class TestData : IAsyncDisposable
    {
        public AtemSwitcher Atem;
        public TestServices Services;

        public TestData()
        {
            Services = new TestServices();
            Atem = new AtemSwitcher("127.0.0.1", 1234, Services);
        }

        public async ValueTask DisposeAsync()
        {
            // If connected: succeed disconnection
            if (Services.ClientFake.RemoteEndPoint is not null)
            {
                Services.ClientFake.SuccessfullyDisconnect();
            }

            await Atem.DisposeAsync();

            // Force stop all action loops
            await Services.DisposeAsync();
        }
    }


    [Test]
    public async Task ConnectAsync()
    {
        await using var data = new TestData();

        var connectTask = data.Atem.ConnectAsync();
        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));

        data.Services.ClientFake.SuccessfullyConnect();
        await connectTask.TimesOut().WithTimeout();

        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await connectTask.WithTimeout();

        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Connected));
        Assert.That(data.Atem.State, Is.Not.Null);
    }

    [Test]
    public async Task ConnectAsync_WithCustomPort_ShouldUseSpecifiedPort()
    {
        // Arrange
        await using var data = new TestData();

        var customPort = 8888;

        // Act
        data.Atem = new("127.0.0.1", customPort, data.Services);
        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        // Assert
        Assert.That(data.Services.ClientFake.RemoteEndPoint?.Port, Is.EqualTo(customPort));
    }

    [Test]
    public async Task ConnectAsync_WhileConnected_ShouldThrow()
    {
        await using var data = new TestData();

        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => data.Atem.ConnectAsync());
        Assert.That(ex!.Message, Does.Contain("Can not connect while"));
    }

    [Test]
    public async Task ConnectAsync_WhileConnecting_ShouldThrow()
    {
        await using var data = new TestData();

        _ = data.Atem.ConnectAsync();

        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Atem.ConnectAsync().WithTimeout());
        Assert.That(ex!.Message, Does.Contain("Can not connect while"));
    }

    [Test]
    public async Task ConnectAsync_WithExceptionFromClient_ShouldThrow()
    {
        await using var data = new TestData();

        var exception = new Exception("Test");
        data.Services.ClientFake.FailConnect(exception);

        var ex = Assert.ThrowsAsync<Exception>(async () => await data.Atem.ConnectAsync().WithTimeout());
        Assert.Multiple(() =>
        {
            Assert.That(ex, Is.SameAs(exception));
            Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
            Assert.That(data.Services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
        });
    }

    [Test]
    public async Task ConnectAsync_DisposeWhileWaitingForInitCommand()
    {
        await using var data = new TestData();

        var connectTask = data.Atem.ConnectAsync();
        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));

        data.Services.ClientFake.SuccessfullyConnect();
        await connectTask.TimesOut().WithTimeout();

        await data.Atem.DisposeAsync().AsTask().WithTimeout();

        Assert.ThrowsAsync<TaskCanceledException>(async () => await connectTask.WithTimeout());

        Assert.That(data.Services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
    }

    [Test]
    public async Task DisconnectAsync()
    {
        await using var data = new TestData();

        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        var disconnectTask = data.Atem.DisconnectAsync();
        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnecting));
        data.Services.ClientFake.SuccessfullyDisconnect();
        await disconnectTask.WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
            Assert.That(data.Services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
        });
    }

    [Test]
    public async Task DisconnectAsync_WithFailureFromClient_ShouldThrow()
    {
        await using var data = new TestData();

        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        var exception = new Exception("Test");

        data.Services.ClientFake.FailDisconnect(exception);
        var ex = Assert.ThrowsAsync<Exception>(async () => await data.Atem.DisconnectAsync().WithTimeout());
        Assert.That(ex, Is.SameAs(exception));
    }

    [Test]
    public async Task DisconnectAsync_WhenNotConnected_ShouldNotChangeAnything()
    {
        await using var data = new TestData();

        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
        Assert.DoesNotThrowAsync(() => data.Atem.DisconnectAsync().WithTimeout());
        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Disconnected));
    }

    [Test]
    public async Task DisconnectAsync_WhileConnecting_ShouldThrow()
    {
        await using var data = new TestData();

        _ = data.Atem.ConnectAsync();
        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => data.Atem.DisconnectAsync().WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while transitioning connection states"));
        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));
    }

    [Test]
    public async Task DisconnectAsync_WhileDisconnecting_ShouldThrow()
    {
        await using var data = new TestData();

        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        var disconnectTask = data.Atem.DisconnectAsync();
        Assert.That(disconnectTask.IsCompleted, Is.False);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => data.Atem.DisconnectAsync().WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while transitioning connection states"));
    }

    [Test]
    public async Task ConnectAsync_AfterDispose_ShouldThrowObjectDisposedException()
    {
        await using var data = new TestData();

        await data.Atem.DisposeAsync().AsTask();
        Assert.ThrowsAsync<ObjectDisposedException>(() => data.Atem.ConnectAsync().WithTimeout());
    }

    [Test]
    public async Task ConnectAsync_StateInitialization_ShouldCreateNewState()
    {
        // Arrange
        await using var data = new TestData();

        var oldState = data.Atem.State;

        // Act
        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        // Assert
        Assert.That(data.Atem.State, Is.TypeOf<AtemSharp.State.AtemState>());
        Assert.That(data.Atem.State, Is.Not.SameAs(oldState));
    }

    [Test]
    public async Task SendCommandAsync()
    {
        await using var data = new TestData();

        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        var command = new AutoTransitionCommand(new MixEffect());
        await data.Atem.SendCommandAsync(command).WithTimeout();

        Assert.That(data.Services.ClientFake.SentCommands, Is.EquivalentTo(new[] { command }));
    }

    [Test]
    public async Task SendCommandAsync_WhileNotConnected_ShouldThrow()
    {
        await using var data = new TestData();

        var sendTask = data.Atem.SendCommandAsync(new MacroActionCommand(new Macro(data.Atem), MacroAction.Run));
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while not connected"));
    }

    [Test]
    public async Task SendCommandsAsync()
    {
        await using var data = new TestData();

        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        MacroActionCommand[] commands = [new(new Macro(data.Atem), MacroAction.Run), new(new Macro(data.Atem), MacroAction.Stop)];
        await data.Atem.SendCommandsAsync(commands).WithTimeout();

        Assert.That(data.Services.ClientFake.SentCommands, Is.EquivalentTo(commands));
    }

    [Test]
    public async Task SendCommandsAsync_WhileNotConnected_ShouldThrow()
    {
        await using var data = new TestData();

        MacroActionCommand[] commands = [new(new Macro(data.Atem), MacroAction.Run), new(new Macro(data.Atem), MacroAction.Stop)];
        var sendTask = data.Atem.SendCommandsAsync(commands);
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
        Assert.That(ex.Message, Contains.Substring("while not connected"));
    }

    [Test]
    public async Task ReceiveCommand_ShouldUpdateState()
    {
        await using var data = new TestData();

        var connectTask = data.Atem.ConnectAsync();
        Assert.That(data.Atem.ConnectionState, Is.EqualTo(ConnectionState.Connecting));

        data.Services.ClientFake.SuccessfullyConnect();
        await connectTask.TimesOut().WithTimeout();

        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await connectTask.WithTimeout();

        data.Atem.Macros.Populate(5);

        var evt = new SemaphoreSlim(0);

        data.Atem.Macros.Player.CurrentlyPlayingChanged += (_, _) => evt.Release();

        data.Services.ClientFake.SimulateReceivedCommand(new MacroRunStatusUpdateCommand()
        {
            IsWaiting = false,
            IsRunning = true,
            MacroIndex = 2,
            Loop = true
        });

        await evt.WaitAsync().WithTimeout();


        Assert.Multiple(() =>
        {
            Assert.That(data.Atem.Macros.Player.CurrentlyPlaying, Is.SameAs(data.Atem.Macros[2]));
            Assert.That(data.Atem.Macros.Player.PlayLooped, Is.True);
            Assert.That(data.Atem.Macros.Player.PlaybackIsWaiting, Is.False);
        });
    }

    [Test]
    public async Task DisposeAsync()
    {
        await using var data = new TestData();

        data.Services.ClientFake.SuccessfullyConnect();
        data.Services.ClientFake.SimulateReceivedCommand(new InitCompleteCommand());
        await data.Atem.ConnectAsync().WithTimeout();

        data.Services.ClientFake.SuccessfullyDisconnect();
        await data.Atem.DisposeAsync().AsTask().WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(data.Services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
            Assert.That(data.Services.ClientFake.DisposeCounter, Is.EqualTo(1));
        });
    }
}
