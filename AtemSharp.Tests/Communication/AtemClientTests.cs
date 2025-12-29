using System.Reflection;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Attributes;
using AtemSharp.Commands;
using AtemSharp.Commands.Macro;
using AtemSharp.Communication;
using AtemSharp.Lib;
using AtemSharp.Tests.TestUtilities;
using NSubstitute;

namespace AtemSharp.Tests.Communication;

[TestFixture]
public class AtemClientTests
{
    private class TestInstances : IAsyncDisposable
    {
        public readonly AtemClient Sut;
        public readonly TestServices Services;

        public TestInstances()
        {
            Services = new TestServices();
            Sut = new AtemClient(Services);
        }

        public async ValueTask DisposeAsync()
        {
            if (Sut.State == ConnectionState.Connected)
            {
                // Succeed the next disconnection, so AtemClient.Dispose finishes disconnecting
                Services.ProtocolFake.SucceedDisconnection();

                await Sut.DisposeAsync();
            }

            await Services.DisposeAsync();
        }
    }

    [Test]
    public async Task ConstructionDoesNotTryToConnectOrDisconnect()
    {
        await using var data = new TestInstances();

        Assert.Multiple(() =>
        {
            Assert.That(data.Services.ProtocolFake.HasConnectionRequest, Is.False);
            Assert.That(data.Services.ProtocolFake.HasDisconnectionRequest, Is.False);
        });
    }

    [Test]
    public async Task Connect()
    {
        await using var data = new TestInstances();

        var connectTask = data.Sut.ConnectAsync("127.0.0.1", 12345);
        await data.Services.ProtocolFake.WaitForConnectionRequestAsync().WithTimeout();
        data.Services.ProtocolFake.SucceedConnection();
        await connectTask.WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(data.Services.ProtocolFake.RemoteEndpoint!.Address.ToString(), Is.EqualTo("127.0.0.1"));
            Assert.That(data.Services.ProtocolFake.RemoteEndpoint.Port, Is.EqualTo(12345));
            Assert.That(data.Services.RunningLoops.Count, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task Connect_WhileConnecting()
    {
        await using var data = new TestInstances();
        var connectTask = data.Sut.ConnectAsync("127.0.0.1", 12345);

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout());

        data.Services.ProtocolFake.SucceedConnection();

        await connectTask.WithTimeout();
    }

    [Test]
    public async Task Connect_WhileConnected()
    {
        await using var data = new TestInstances();
        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout());
    }

    [Test]
    public async Task Connect_WhileDisconnecting()
    {
        await using var data = new TestInstances();
        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout());

        data.Services.ProtocolFake.SucceedDisconnection();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task Disconnect()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();
        await data.Services.ProtocolFake.WaitForDisconnectionRequestAsync().WithTimeout();
        data.Services.ProtocolFake.SucceedDisconnection();
        await disconnectTask.WithTimeout();

        Assert.Multiple(() =>
        {
            Assert.That(data.Services.ProtocolFake.RemoteEndpoint, Is.Null);
            Assert.That(data.Services.RunningLoops, Is.All.Matches<ActionLoop>(x => !x.IsRunning));
        });
    }

    [Test]
    public async Task Disconnect_CancelsSendTasks()
    {
        await using var data = new TestInstances();
        var expectedPayload = new byte[] { 1, 2, 3, 4 };
        data.Services.PacketBuilder.GetPackets().Returns(new List<byte[]> { expectedPayload });

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var sendTask = data.Sut.SendCommandAsync(MacroActionCommand.Stop());

        data.Services.ProtocolFake.SucceedDisconnection();
        await data.Sut.DisconnectAsync().WithTimeout();

        Assert.ThrowsAsync<TaskCanceledException>(async () => await sendTask.WithTimeout());
    }

    [Test]
    public async Task Disconnect_WhileConnecting()
    {
        await using var data = new TestInstances();

        _ = data.Sut.ConnectAsync("127.0.0.1", 12345);
        await data.Services.ProtocolFake.WaitForConnectionRequestAsync().WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.DisconnectAsync().WithTimeout());
    }

    [Test]
    public async Task Disconnect_WhileNotConnected()
    {
        await using var data = new TestInstances();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.DisconnectAsync().WithTimeout());
    }

    [Test]
    public async Task Disconnect_WhileDisconnecting()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();
        await data.Services.ProtocolFake.WaitForDisconnectionRequestAsync().WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await data.Sut.DisconnectAsync().WithTimeout());

        data.Services.ProtocolFake.SucceedDisconnection();
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task Disconnect_IgnoresProtocolExceptions()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var disconnectTask = data.Sut.DisconnectAsync();
        await data.Services.ProtocolFake.WaitForDisconnectionRequestAsync().WithTimeout();
        data.Services.ProtocolFake.FailDisconnection(new Exception("My Exception"));
        await disconnectTask.WithTimeout();
    }

    [Test]
    public async Task SendCommand()
    {
        await using var data = new TestInstances();
        var expectedPayload = new byte[] { 1, 2, 3, 4 };
        data.Services.PacketBuilder.GetPackets().Returns(new List<byte[]> { expectedPayload });

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var sendTask = data.Sut.SendCommandAsync(MacroActionCommand.Stop());
        var packet = await data.Services.ProtocolFake.GetSentPacket().WithTimeout();

        VerifySentPacket(packet, expectedPayload);

        Assert.That(sendTask.IsCompleted, Is.False);
        await data.Services.ProtocolFake.AckPacket(packet).WithTimeout();
        await sendTask.WithTimeout();
    }

    [Test]
    public async Task SendCommand_Twice()
    {
        await using var data = new TestInstances();
        var expectedPayload = new byte[] { 1, 2, 3, 4 };
        data.Services.PacketBuilder.GetPackets().Returns(new List<byte[]> { expectedPayload });

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        Task[] sendTasks =
        [
            data.Sut.SendCommandAsync(MacroActionCommand.Stop()),
            data.Sut.SendCommandAsync(MacroActionCommand.Stop())
        ];

        var packet = await data.Services.ProtocolFake.GetSentPacket().WithTimeout();
        await data.Services.ProtocolFake.AckPacket(packet).WithTimeout();
        await sendTasks[0].WithTimeout();

        packet = await data.Services.ProtocolFake.GetSentPacket().WithTimeout();
        await data.Services.ProtocolFake.AckPacket(packet).WithTimeout();
        await sendTasks[1].WithTimeout();
    }

    [Test]
    public async Task SendCommands_Two()
    {
        await using var data = new TestInstances();
        var expectedPayload = new byte[] { 1, 2, 3, 4 };
        data.Services.PacketBuilder.GetPackets().Returns(new List<byte[]> { expectedPayload });

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var sendTask = data.Sut.SendCommandsAsync([MacroActionCommand.Stop(), MacroActionCommand.Stop()]);

        var packet = await data.Services.ProtocolFake.GetSentPacket().WithTimeout();

        await data.Services.ProtocolFake.AckPacket(packet).WithTimeout();
        await sendTask.WithTimeout();
    }

    [Test]
    public async Task SendCommands_None()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var sendTask = data.Sut.SendCommandsAsync([]);
        await sendTask.WithTimeout();
    }

    [Test]
    public async Task AckingUnsentCommand_DoesNotPoseAProblem()
    {
        await using var data = new TestInstances();

        var expectedPayload = new byte[] { 1, 2, 3, 4 };
        data.Services.PacketBuilder.GetPackets().Returns(new List<byte[]> { expectedPayload });

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();


        var sendTask = data.Sut.SendCommandAsync(MacroActionCommand.Stop());
        var packet = await data.Services.ProtocolFake.GetSentPacket().WithTimeout();

        VerifySentPacket(packet, expectedPayload);

        Assert.That(sendTask.IsCompleted, Is.False);
        await data.Services.ProtocolFake.AckPacket(new AtemPacket { TrackingId = 123 }).WithTimeout();
        await sendTask.TimesOut();

        await data.Services.ProtocolFake.AckPacket(packet).WithTimeout();
        await sendTask.WithTimeout();
    }

    [Test]
    public async Task SendCommand_WhileNotConnected()
    {
        await using var data = new TestInstances();

        var sendTask = data.Sut.SendCommandAsync(MacroActionCommand.Stop()).WithTimeout();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await sendTask.WithTimeout());
    }

    [Test]
    public async Task SendCommands_WithoutCommands()
    {
        await using var data = new TestInstances();
    }

    [Test]
    public async Task ReceiveCommand_SingleCommand()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var command = new InitCompleteCommand();
        var payload = new byte[9];
        payload.WriteUInt16BigEndian((ushort)payload.Length, 0);
        payload.WriteString(command.GetType().GetCustomAttribute<CommandAttribute>()?.RawName, 4, 5);

        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        data.Services.CommandParserFake.CommandsToReturn.Enqueue(new InitCompleteCommand());

        var receiveTask = data.Services.ProtocolFake.ReceivePacketAsync(packet);
        var receivedCommand = await data.Sut.ReceivedCommands.ReceiveAsync().WithTimeout();

        Assert.That(receivedCommand, Is.InstanceOf<InitCompleteCommand>());

        await receiveTask.WithTimeout();

        Assert.That(data.Services.CommandParserFake.ParsedData[0].Item1, Is.EqualTo("InCm"));

        // Ack-ing is not handled in the AtemClient but in the AtemProtocol
    }

    [Test]
    public async Task ReceiveTooSmallPacket()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var payload = new byte[4];
        payload.WriteUInt16BigEndian(4, 0);
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        var receiveTask = data.Services.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();

        await receiveTask.WithTimeout();
    }

    [Test]
    public async Task ReceiveSmallerPayloadThanSizeIndicated()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var payload = new byte[8];
        payload.WriteUInt16BigEndian(10, 0);
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        var receiveTask = data.Services.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();

        await receiveTask.WithTimeout();
    }

    [Test]
    public async Task ReceivePacketWithIndicatedSizeSmallerThanHeader()
    {
        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();

        var payload = new byte[8];
        payload.WriteUInt16BigEndian(4, 0);
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
        };

        var receiveTask = data.Services.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();

        await receiveTask.WithTimeout();
    }

    [Test]
    public async Task ReceiveCommand_IgnoresCommandsAfterCommandThatIsShorterThanHeader()
    {
        var payload = new byte[12];
        payload.WriteUInt16BigEndian(4, 0);
        payload.WriteUInt16BigEndian(8, 4);
        payload[8] = (byte)'I';
        payload[9] = (byte)'n';
        payload[10] = (byte)'C';
        payload[11] = (byte)'m';

        var packet = new AtemPacket(payload);

        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();
        var receiveTask = data.Services.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();
        await receiveTask.WithTimeout();
    }

    [Test]
    public async Task ReceiveCommand_IgnoresCommandsAfterCommandThatIsLongerThanPayload()
    {
        var payload = new byte[12];
        payload.WriteUInt16BigEndian(14, 0);
        payload.WriteUInt16BigEndian(8, 4);
        payload[8] = (byte)'I';
        payload[9] = (byte)'n';
        payload[10] = (byte)'C';
        payload[11] = (byte)'m';

        var packet = new AtemPacket(payload);

        await using var data = new TestInstances();

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();
        var receiveTask = data.Services.ProtocolFake.ReceivePacketAsync(packet);
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();
        await receiveTask.WithTimeout();
    }

    [Test]
    public async Task ReceiveCommand_TwoCommandsInOnePayload()
    {
        var payload = new byte[8];
        payload.WriteUInt16BigEndian(8, 0);
        payload[4] = (byte)'I';
        payload[5] = (byte)'n';
        payload[6] = (byte)'C';
        payload[7] = (byte)'m';


        var packet = new AtemPacket(payload.Concat(payload).ToArray());

        await using var data = new TestInstances();
        data.Services.CommandParserFake.CommandsToReturn.Enqueue(new InitCompleteCommand());
        data.Services.CommandParserFake.CommandsToReturn.Enqueue(new InitCompleteCommand());

        data.Services.ProtocolFake.SucceedConnection();
        await data.Sut.ConnectAsync("127.0.0.1", 12345).WithTimeout();
        var receiveTask = data.Services.ProtocolFake.ReceivePacketAsync(packet);
        var receivedCommand = await data.Sut.ReceivedCommands.ReceiveAsync().WithTimeout();
        Assert.That(receivedCommand, Is.InstanceOf<InitCompleteCommand>());
        receivedCommand = await data.Sut.ReceivedCommands.ReceiveAsync().WithTimeout();
        Assert.That(receivedCommand, Is.InstanceOf<InitCompleteCommand>());
        await data.Sut.ReceivedCommands.ReceiveAsync().TimesOut().WithTimeout();
        await receiveTask.WithTimeout();

        Assert.That(data.Services.CommandParserFake.ParsedData.Count, Is.EqualTo(2));
    }

    public static void VerifySentPacket(AtemPacket packet, byte[] expectedBytes)
    {
        Assert.That(packet.Payload, Is.EquivalentTo(expectedBytes));
    }
}
