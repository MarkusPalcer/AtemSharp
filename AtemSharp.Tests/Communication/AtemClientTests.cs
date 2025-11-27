using System.Threading.Tasks.Dataflow;
using AtemSharp.Commands;
using AtemSharp.Commands.Macro;
using AtemSharp.Communication;
using AtemSharp.Lib;
using AtemSharp.State.Info;
using AtemSharp.State.Macro;
using AtemSharp.Tests.TestUtilities;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Tests.Communication;

[TestFixture]
public class AtemClientTests
{
    private class TestInstances : IAsyncDisposable
    {
        public AtemClient Sut;
        public ILoggerFactory LoggerFactory;
        public AtemProtocolFake ProtocolFake;
        public TestActionLoopFactory ActionLoopFactory;

        public TestInstances()
        {
            ProtocolFake = new AtemProtocolFake();
            LoggerFactory = new LoggerFactory();
            ActionLoopFactory = new TestActionLoopFactory();
            Sut = new AtemClient(LoggerFactory, () => ProtocolFake, ActionLoopFactory);
        }

        public async ValueTask DisposeAsync()
        {
            if (Sut.State == ConnectionState.Connected)
            {
                // Succeed the next disconnection, so AtemClient.Dispose finishes disconnecting
                ProtocolFake.SucceedDisconnection();

                await Sut.DisposeAsync();
            }

            ActionLoopFactory.Dispose();
        }
    }

    [Test]
    public async Task ConstructionDoesNotTryToConnectOrDisconnect()
    {
        await using var data = new TestInstances();

        Assert.Multiple(() =>
        {
            Assert.That(data.ProtocolFake.HasConnectionRequest, Is.False);
            Assert.That(data.ProtocolFake.HasDisconnectionRequest, Is.False);
        });
    }

    [Test]
    public async Task Connect()
    {
        await using var data = new TestInstances();

        var connectTask = data.Sut.ConnectAsync("127.0.0.1", 12345);
        await data.ProtocolFake.WaitForConnectionRequestAsync().WithTimeout();
        data.ProtocolFake.SucceedConnection();
        await connectTask.WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(data.ProtocolFake.RemoteEndpoint!.Address.ToString(), Is.EqualTo("127.0.0.1"));
            Assert.That(data.ProtocolFake.RemoteEndpoint.Port, Is.EqualTo(12345));
            Assert.That(data.ActionLoopFactory.RunningLoops.Count, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task ConnectWhileConnecting()
    {
        await using var data = new TestInstances();
        var connectTask = data.Sut.ConnectAsync("127.0.0.1", 12345);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout());

        data.ProtocolFake.SucceedConnection();

        await connectTask.WithTimeout();
    }

    [Test]
    public async Task ConnectWhileConnected()
    {
        await using var data = new TestInstances();
        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout());
    }

    [Test]
    public async Task ConnectWhileDisconnecting()
    {
        await using var data = new TestInstances();
        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout());

        data.ProtocolFake.SucceedDisconnection();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task Disconnect()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();
        await data.ProtocolFake.WaitForDisconnectionRequestAsync().WithTimeout();
        data.ProtocolFake.SucceedDisconnection();
        await disconnectTask.WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(data.ProtocolFake.RemoteEndpoint, Is.Null);
            Assert.That(data.ActionLoopFactory.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
        });
    }

    [Test]
    public async Task DisconnectWhileConnecting()
    {
        await using var data = new TestInstances();

        var connectTask = data.Sut.ConnectAsync("127.0.0.1", 12345);
        await data.ProtocolFake.WaitForConnectionRequestAsync().WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.DisconnectAsync().WithTimeout());

        data.ProtocolFake.SucceedConnection();
        await connectTask.WithTimeout();
    }

    [Test]
    public async Task DisconnectWhileNotConnected()
    {
        await using var data = new TestInstances();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.DisconnectAsync().WithTimeout());
    }

    [Test]
    public async Task DisconnectWhileDisconnecting()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();
        await data.ProtocolFake.WaitForDisconnectionRequestAsync().WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.DisconnectAsync().WithTimeout());

        data.ProtocolFake.SucceedDisconnection();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task Disconnect_IgnoresProtocolExceptions()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();
        await data.ProtocolFake.WaitForDisconnectionRequestAsync().WithTimeout();
        data.ProtocolFake.FailDisconnection(new Exception("My Exception"));
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task SendCommand()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var command = new MacroActionCommand(new Macro { Id = 0 }, MacroAction.Run);
        var sendTask = data.Sut.SendCommandAsync(command);
        var packet = await data.ProtocolFake.GetSentPacket().WithTimeout();

        VerifyPacketPayload(packet, command);

        Assert.That(sendTask.IsCompleted, Is.False);
        await data.ProtocolFake.AckPacket(packet).WithTimeout();
        await sendTask.WithTimeout();
    }

    [Test]
    public async Task AckingUnsentCommand_DoesNotPoseAProblem()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var command = new MacroActionCommand(new Macro { Id = 0 }, MacroAction.Run);
        var sendTask = data.Sut.SendCommandAsync(command);
        var packet = await data.ProtocolFake.GetSentPacket().WithTimeout();

        VerifyPacketPayload(packet, command);

        Assert.That(sendTask.IsCompleted, Is.False);
        await data.ProtocolFake.AckPacket(new AtemPacket { TrackingId = 123}).WithTimeout();
        await sendTask.TimesOut();

        await data.ProtocolFake.AckPacket(packet).WithTimeout();
        await sendTask.WithTimeout();
    }

    [Test]
    public async Task SendCommand_WhileNotConnected()
    {
        await using var data = new TestInstances();

        var command = new MacroActionCommand(new Macro { Id = 0 }, MacroAction.Run);
        var sendTask = data.Sut.SendCommandAsync(command).WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
    }

    [Test]
    public async Task ReceiveCommand()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var command = new InitCompleteCommand();
        var payload = new byte[9];
        payload.WriteUInt16BigEndian((ushort)payload.Length, 0);
        payload.WriteString(command.GetRawName(), 4, 5);

        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        var receiveTask = data.ProtocolFake.ReceivePacketAsync(packet);
        var receivedCommand = await data.Sut.ReceivedCommands.ReceiveAsync().WithTimeout();

        Assert.That(receivedCommand, Is.InstanceOf<InitCompleteCommand>());

        await receiveTask.WithTimeout();

        // Ack-ing is not handled in the AtemClient but in the AtemProtocol
    }

    [Test]
    public async Task ReceiveTooSmallPacket()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var payload = new byte[4];
        payload.WriteUInt16BigEndian(4, 0);
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        var receiveTask = data.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();

        await receiveTask.WithTimeout();
    }

    [Test]
    public async Task ReceiveSmallerPayloadThanSizeIndicated()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var payload = new byte[8];
        payload.WriteUInt16BigEndian(10, 0);
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        var receiveTask = data.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();

        await receiveTask.WithTimeout();
    }

    [Test]
    public async Task ReceivePacketWithIndicatedSizeSmallerThanHeader()
    {
        await using var data = new TestInstances();

        data.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var payload = new byte[8];
        payload.WriteUInt16BigEndian(4, 0);
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        var receiveTask = data.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();

        await receiveTask.WithTimeout();
    }

    public static void VerifyPacketPayload(AtemPacket packet, SerializedCommand expectedCommand)
    {
        Assert.Multiple(() =>
        {
            var sentData = (ReadOnlySpan<byte>)packet.Payload.AsSpan();
            var expectedBytes = expectedCommand.Serialize(ProtocolVersion.Unknown);


            // Bytes 1+2: Length
            Assert.That(sentData.ReadUInt16BigEndian(0), Is.EqualTo(expectedBytes.Length + Constants.AtemConstants.CommandHeaderSize));

            // Bytes 3+4: Empty

            // Bytes 4-8: Command name
            Assert.That(sentData.ReadString(4, 4), Is.EqualTo(expectedCommand.GetRawName()));

            // Rest: Command Data
            Assert.That(sentData[8..].ToArray(), Is.EquivalentTo(expectedBytes));
        });
    }
}
