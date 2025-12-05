using System.Net;
using System.Threading.Tasks.Dataflow;
using AtemSharp.Communication;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Communication;

[TestFixture]
public class AtemProtocolTests
{
    private const ushort SessionId = 1234;

    [Test]
    public async Task Connect()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);

        var connectTask = sut.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9110));

        var helloPacket = await services.UdpFake.SentData.ReceiveAsync().WithTimeout();
        Assert.Multiple(() =>
        {
            Assert.That(helloPacket, Is.EquivalentTo(AtemProtocol.CommandConnectHello));
            Assert.That(connectTask.IsCompleted, Is.False);
        });

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var sessionPacket = new AtemPacket
        {
            Flags = PacketFlag.NewSessionId,
            SessionId = SessionId
        };

        await services.UdpFake.SimulateReceive(sessionPacket.ToBytes());

        await connectTask.WithTimeout();

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var ackPacket = AtemPacket.FromBytes(await services.UdpFake.SentData.ReceiveAsync().WithTimeout());
        Assert.Multiple(() =>
        {
            Assert.That(ackPacket.Flags, Is.EqualTo(PacketFlag.AckReply));
            Assert.That(ackPacket.SessionId, Is.EqualTo(SessionId));
        });

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));
    }

    [Test]
    public async Task Connect_WhileConnected()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);
        await EstablishConnection(sut, services);

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => sut.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9110)).WithTimeout());
        Assert.That(ex.Message, Contains.Substring("Can only connect while not connected"));
    }

    [Test]
    public async Task Disconnect_WhileDisconnected()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);

        await sut.DisconnectAsync().WithTimeout();
    }

    [Test]
    public async Task SendPacket_HappyPath()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);
        await EstablishConnection(sut, services);

        byte[] payload = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        var packetToSend = new AtemPacket
        {
            Flags = PacketFlag.AckRequest,
            Payload = payload,
            TrackingId = 42069
        };

        await sut.SendPacketsAsync([packetToSend]).WithTimeout();

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var sentData = await services.UdpFake.SentData.ReceiveAsync().WithTimeout();
        var sentPacket = AtemPacket.FromBytes(sentData);

        Assert.That(sentPacket.Payload, Is.EquivalentTo(payload));
        Assert.That(sentPacket.Flags, Is.EqualTo(PacketFlag.AckRequest));
        Assert.That(sentPacket.SessionId, Is.EqualTo(SessionId));

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var ackPacket = AtemPacket.CreateAck(SessionId, sentPacket.PacketId);
        await services.UdpFake.SimulateReceive(ackPacket.ToBytes());

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        Assert.That(await sut.AckedTrackingIds.ReceiveAsync().WithTimeout(), Is.EqualTo(42069));
    }

    [Test]
    public async Task PacketIdOverflow()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);
        await EstablishConnection(sut, services);

        List<ushort> packetIds = [];

        for (var i = 0; i < AtemProtocol.MaxPacketId + 1; i++)
        {
            byte[] payload = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

            var packetToSend = new AtemPacket
            {
                Flags = PacketFlag.AckRequest,
                Payload = payload,
                TrackingId = 42069
            };

            await sut.SendPacketsAsync([packetToSend]).WithTimeout();

            packetIds.Add(packetToSend.PacketId);
        }

        Assert.That(packetIds, Is.EquivalentTo(Enumerable.Range(1, AtemProtocol.MaxPacketId - 1).Concat([0, 1])));
    }

    [Test]
    public async Task ReceiveCommand_HappyPath()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);
        await EstablishConnection(sut, services);

        byte[] payload = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        var packetToReceive = new AtemPacket
        {
            PacketId = 1,
            Flags = PacketFlag.AckRequest,
            Payload = payload,
            SessionId = SessionId
        };

        await services.UdpFake.SimulateReceive(packetToReceive.ToBytes());

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var receivedPacket = await sut.ReceivedPackets.ReceiveAsync().WithTimeout();
        Assert.Multiple(() =>
        {
            Assert.That(receivedPacket.PacketId, Is.EqualTo(packetToReceive.PacketId));
            Assert.That(receivedPacket.Flags, Is.EqualTo(PacketFlag.AckRequest));
            Assert.That(receivedPacket.Payload, Is.EquivalentTo(payload));
            Assert.That(receivedPacket.SessionId, Is.EqualTo(SessionId));
        });

        await services.VirtualTime.AdvanceBy(AtemProtocol.AckDelay + TimeSpan.FromMilliseconds(1));

        var ackPacket = AtemPacket.FromBytes(await services.UdpFake.SentData.ReceiveAsync().WithTimeout());
        Assert.Multiple(() =>
        {
            Assert.That(ackPacket.Flags, Is.EqualTo(PacketFlag.AckReply));
            Assert.That(ackPacket.SessionId, Is.EqualTo(SessionId));
            Assert.That(ackPacket.AckPacketId, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task ReceiveCommand_ReceiveMultiple_OnlyAckOnce()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);
        await EstablishConnection(sut, services);

        var packetToReceive = new AtemPacket
        {
            PacketId = 1,
            Flags = PacketFlag.AckRequest,
            Payload = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
            SessionId = SessionId
        };

        await services.UdpFake.SimulateReceive(packetToReceive.ToBytes());

        var receivedPacket = await sut.ReceivedPackets.ReceiveAsync().WithTimeout();
        Assert.Multiple(() =>
        {
            Assert.That(receivedPacket.PacketId, Is.EqualTo(1));
            Assert.That(receivedPacket.Flags, Is.EqualTo(PacketFlag.AckRequest));
            Assert.That(receivedPacket.Payload, Is.EquivalentTo((byte[])[1, 2, 3, 4, 5, 6, 7, 8, 9, 10]));
            Assert.That(receivedPacket.SessionId, Is.EqualTo(SessionId));
        });

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        packetToReceive = new AtemPacket
        {
            PacketId = 2,
            Flags = PacketFlag.AckRequest,
            Payload = [11, 12, 13, 14, 15, 16, 17, 18, 19, 110],
            SessionId = SessionId
        };

        await services.UdpFake.SimulateReceive(packetToReceive.ToBytes());

        receivedPacket = await sut.ReceivedPackets.ReceiveAsync().WithTimeout();
        Assert.Multiple(() =>
        {
            Assert.That(receivedPacket.PacketId, Is.EqualTo(2));
            Assert.That(receivedPacket.Flags, Is.EqualTo(PacketFlag.AckRequest));
            Assert.That(receivedPacket.Payload, Is.EquivalentTo((byte[])[11, 12, 13, 14, 15, 16, 17, 18, 19, 110]));
            Assert.That(receivedPacket.SessionId, Is.EqualTo(SessionId));
        });

        await services.VirtualTime.AdvanceBy(AtemProtocol.AckDelay + TimeSpan.FromMilliseconds(1));

        var ackPacket = AtemPacket.FromBytes(await services.UdpFake.SentData.ReceiveAsync().WithTimeout());
        Assert.Multiple(() =>
        {
            Assert.That(ackPacket.Flags, Is.EqualTo(PacketFlag.AckReply));
            Assert.That(ackPacket.SessionId, Is.EqualTo(SessionId));
            Assert.That(ackPacket.AckPacketId, Is.EqualTo(2));
        });
    }

    [Test]
    public async Task ReceiveCommand_ReceiveMany_CircumventAckTimer()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);
        await EstablishConnection(sut, services);

        for (byte i = 0; i < AtemProtocol.MaxPacketPerAck; i++)
        {
            byte[] payload = [(byte)(i+1), (byte)(i+2), (byte)(i+3), (byte)(i+4), (byte)(i+5), (byte)(i+6), (byte)(i+7), (byte)(i+8), (byte)(i+9), (byte)(i+10)];
            var packetToReceive = new AtemPacket
            {
                PacketId = (ushort)(i+1),
                Flags = PacketFlag.AckRequest,
                Payload = payload,
                SessionId = SessionId
            };

            await services.UdpFake.SimulateReceive(packetToReceive.ToBytes());

            var receivedPacket = await sut.ReceivedPackets.ReceiveAsync().WithTimeout();
            Assert.Multiple(() =>
            {
                Assert.That(receivedPacket.PacketId, Is.EqualTo(i+1));
                Assert.That(receivedPacket.Flags, Is.EqualTo(PacketFlag.AckRequest));
                Assert.That(receivedPacket.Payload, Is.EquivalentTo(payload));
                Assert.That(receivedPacket.SessionId, Is.EqualTo(SessionId));
            });
        }

        await services.VirtualTime.AdvanceBy(AtemProtocol.AckDelay + TimeSpan.FromMilliseconds(1));

        var ackPacket = AtemPacket.FromBytes(await services.UdpFake.SentData.ReceiveAsync().WithTimeout());
        Assert.Multiple(() =>
        {
            Assert.That(ackPacket.Flags, Is.EqualTo(PacketFlag.AckReply));
            Assert.That(ackPacket.SessionId, Is.EqualTo(SessionId));
            Assert.That(ackPacket.AckPacketId, Is.EqualTo(AtemProtocol.MaxPacketPerAck));
        });
    }

    [Test]
    public async Task ReceivePacket_WithoutPayload_IsNotPropagated()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);

        await EstablishConnection(sut, services);
        var packet = new AtemPacket
        {
            PacketId = 1,
            Flags = PacketFlag.AckRequest,
            SessionId = SessionId
        };

        await services.UdpFake.SimulateReceive(packet.ToBytes());
        await sut.ReceivedPackets.ReceiveAsync().TimesOut().WithTimeout();
    }

    [Test]
    public async Task ReceiveRetransmitRequest()
    {
        await using var services = new TestServices();
        await using var sut = new AtemProtocol(services);
        await EstablishConnection(sut, services);

        byte[] payload = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        var packetToSend = new AtemPacket
        {
            Flags = PacketFlag.AckRequest,
            Payload = payload,
            TrackingId = 42069
        };

        await sut.SendPacketsAsync([packetToSend]).WithTimeout();

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var originallySentData = await services.UdpFake.SentData.ReceiveAsync().WithTimeout();

        var resendRequest = new AtemPacket
        {
            Flags = PacketFlag.RetransmitRequest,
            PacketId = 1,
            RetransmitFromPacketId = packetToSend.PacketId
        };

        await services.UdpFake.SimulateReceive(resendRequest.ToBytes());

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        var retransmittedData = await services.UdpFake.SentData.ReceiveAsync().WithTimeout();

        Assert.That(retransmittedData, Is.EquivalentTo(originallySentData));
    }

    private async Task EstablishConnection(AtemProtocol sut, TestServices services)
    {

        var connectTask = sut.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9110));
        var sessionPacket = new AtemPacket();

        // Consume Hello Packet
        await services.UdpFake.SentData.ReceiveAsync().WithTimeout();

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        // Send new session ID
        sessionPacket.Flags = PacketFlag.NewSessionId;
        sessionPacket.SessionId = SessionId;
        await services.UdpFake.SimulateReceive(sessionPacket.ToBytes());

        // Connection established
        await connectTask.WithTimeout();

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));

        // Consume Ack Packet
        await services.UdpFake.SentData.ReceiveAsync().WithTimeout();

        await services.VirtualTime.AdvanceBy(TimeSpan.FromMilliseconds(1));
    }
}
