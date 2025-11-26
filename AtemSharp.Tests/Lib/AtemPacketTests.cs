using AtemSharp.Communication;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class AtemPacketTests
{
    [Test]
    public void BasicPacketCreation_ShouldCreateValidPacket()
    {
        // Arrange
        var payload = new byte[] { 0x01, 0x02, 0x03, 0x04 };

        // Act
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
            SessionId = 0x1234,
            PacketId = 0x5678
        };

        // Assert
        Assert.That(packet.IsValid(), Is.True);
        Assert.That(packet.Flags, Is.EqualTo(PacketFlag.AckRequest));
        Assert.That(packet.SessionId, Is.EqualTo(0x1234));
        Assert.That(packet.PacketId, Is.EqualTo(0x5678));
        Assert.That(packet.Payload, Is.EqualTo(payload));
        Assert.That(packet.Length, Is.EqualTo(12 + payload.Length));
    }

    [Test]
    public void ToBytes_ShouldSerializeCorrectly()
    {
        // Arrange
        var payload = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        var packet = new AtemPacket(payload)
        {
            Flags = PacketFlag.AckRequest,
            SessionId = 0x1234,
            PacketId = 0x5678
        };

        // Act
        var serialized = packet.ToBytes();

        // Assert
        Assert.That(serialized.Length, Is.EqualTo(16)); // 12 byte header + 4 byte payload

        // Check flags and length in first two bytes
        var flagsAndLength = (ushort)((serialized[0] << 8) | serialized[1]);
        const ushort expectedFlagsAndLength = ((int)PacketFlag.AckRequest << 11) | 16;
        Assert.That(flagsAndLength, Is.EqualTo(expectedFlagsAndLength));

        // Check session ID
        var sessionId = (ushort)((serialized[2] << 8) | serialized[3]);
        Assert.That(sessionId, Is.EqualTo(0x1234));

        // Check packet ID
        var packetId = (ushort)((serialized[10] << 8) | serialized[11]);
        Assert.That(packetId, Is.EqualTo(0x5678));

        // Check payload
        var actualPayload = new byte[4];
        Array.Copy(serialized, 12, actualPayload, 0, 4);
        Assert.That(actualPayload, Is.EqualTo(payload));
    }

    [Test]
    public void FromBytes_ShouldParseCorrectly()
    {
        // Arrange
        var rawData = new byte[16];

        // Flags (AckRequest = 0x01) shifted left 11 positions + Length (16)
        ushort flagsAndLength = ((int)PacketFlag.AckRequest << 11) | 16;
        rawData[0] = (byte)(flagsAndLength >> 8);
        rawData[1] = (byte)(flagsAndLength & 0xFF);

        // Session ID (0x1234)
        rawData[2] = 0x12;
        rawData[3] = 0x34;

        // Ack Packet ID (0x5678)
        rawData[4] = 0x56;
        rawData[5] = 0x78;

        // Reserved (0x9ABC)
        rawData[6] = 0x9A;
        rawData[7] = 0xBC;

        // Reserved (0x0000)
        rawData[8] = 0x00;
        rawData[9] = 0x00;

        // Packet ID (0xDEF0)
        rawData[10] = 0xDE;
        rawData[11] = 0xF0;

        // Payload
        rawData[12] = 0xAA;
        rawData[13] = 0xBB;
        rawData[14] = 0xCC;
        rawData[15] = 0xDD;

        // Act
        var parsedPacket = AtemPacket.FromBytes(rawData);

        // Assert
        Assert.That(parsedPacket.IsValid(), Is.True);
        Assert.That(parsedPacket.Flags, Is.EqualTo(PacketFlag.AckRequest));
        Assert.That(parsedPacket.Length, Is.EqualTo(16));
        Assert.That(parsedPacket.SessionId, Is.EqualTo(0x1234));
        Assert.That(parsedPacket.AckPacketId, Is.EqualTo(0x5678));
        Assert.That(parsedPacket.RetransmitFromPacketId, Is.EqualTo(0x9ABC));
        Assert.That(parsedPacket.PacketId, Is.EqualTo(0xDEF0));

        var expectedPayload = new byte[] { 0xAA, 0xBB, 0xCC, 0xDD };
        Assert.That(parsedPacket.Payload, Is.EqualTo(expectedPayload));
    }

    [Test]
    public void CreateAck_ShouldCreateValidAckPacket()
    {
        // Act
        var ackPacket = AtemPacket.CreateAck(0x1234, 0x5678);

        // Assert
        Assert.That(ackPacket.HasFlag(PacketFlag.AckReply), Is.True);
        Assert.That(ackPacket.SessionId, Is.EqualTo(0x1234));
        Assert.That(ackPacket.AckPacketId, Is.EqualTo(0x5678));
        Assert.That(ackPacket.Payload.Length, Is.EqualTo(0)); // ACK packets have no payload

        // The packet should be valid after calling ToBytes() which sets the length
        var bytes = ackPacket.ToBytes();
        Assert.That(bytes.Length, Is.EqualTo(12)); // Header only
        Assert.That(ackPacket.IsValid(), Is.True);
    }

    [Test]
    public void CreateHello_ShouldCreateValidHelloPacket()
    {
        // Act
        var helloPacket = AtemPacket.CreateHello();

        // Assert
        Assert.That(helloPacket.IsValid(), Is.True);
        Assert.That(helloPacket.HasFlag(PacketFlag.NewSessionId), Is.True);
        Assert.That(helloPacket.HasFlag(PacketFlag.AckRequest), Is.True);
        Assert.That(helloPacket.SessionId, Is.EqualTo(0x0000)); // Hello packets start with session ID 0
        Assert.That(helloPacket.Payload.Length, Is.EqualTo(8)); // Hello payload is 8 bytes

        // Check hello payload matches expected pattern
        var expectedHelloPayload = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        Assert.That(helloPacket.Payload, Is.EqualTo(expectedHelloPayload));
    }

    [Test]
    public void HasFlag_ShouldDetectFlagsCorrectly()
    {
        // Arrange
        var packet = new AtemPacket([])
        {
            Flags = PacketFlag.AckRequest | PacketFlag.NewSessionId
        };

        // Act & Assert
        Assert.That(packet.HasFlag(PacketFlag.AckRequest), Is.True);
        Assert.That(packet.HasFlag(PacketFlag.NewSessionId), Is.True);
        Assert.That(packet.HasFlag(PacketFlag.AckReply), Is.False);
        Assert.That(packet.HasFlag(PacketFlag.IsRetransmit), Is.False);
        Assert.That(packet.HasFlag(PacketFlag.RetransmitRequest), Is.False);
    }

    [Test]
    public void RoundTripSerialization_ShouldPreserveAllData()
    {
        // Arrange
        var originalPacket = new AtemPacket([0xAA, 0xBB, 0xCC])
        {
            Flags = PacketFlag.AckRequest | PacketFlag.NewSessionId,
            SessionId = 0x1234,
            AckPacketId = 0x5678,
            RetransmitFromPacketId = 0x9ABC,
            PacketId = 0xDEF0
        };

        // Act
        var serialized = originalPacket.ToBytes();
        var parsedBack = AtemPacket.FromBytes(serialized);

        // Assert
        Assert.That(parsedBack.Flags, Is.EqualTo(originalPacket.Flags));
        Assert.That(parsedBack.Length, Is.EqualTo(originalPacket.Length));
        Assert.That(parsedBack.SessionId, Is.EqualTo(originalPacket.SessionId));
        Assert.That(parsedBack.AckPacketId, Is.EqualTo(originalPacket.AckPacketId));
        Assert.That(parsedBack.RetransmitFromPacketId, Is.EqualTo(originalPacket.RetransmitFromPacketId));
        Assert.That(parsedBack.PacketId, Is.EqualTo(originalPacket.PacketId));
        Assert.That(parsedBack.Payload, Is.EqualTo(originalPacket.Payload));
    }

    [Test]
    public void IsValid_ShouldReturnTrueForValidPackets()
    {
        // Arrange
        var validPacket = new AtemPacket([0x01, 0x02]);

        // Act & Assert
        Assert.That(validPacket.IsValid(), Is.True);
    }

    [Test]
    public void FromBytes_WithInsufficientData_ShouldThrowException()
    {
        // Arrange
        var tooShortData = new byte[10]; // Less than 12 bytes header

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AtemPacket.FromBytes(tooShortData));
    }

    [Test]
    public void FromBytes_WithInconsistentLength_ShouldThrowException()
    {
        // Arrange
        var data = new byte[16];
        // Set length field to 20 but only provide 16 bytes
        data[0] = 0x00;
        data[1] = 0x14; // Length = 20

        // Act & Assert
        Assert.Throws<ArgumentException>(() => AtemPacket.FromBytes(data));
    }

    [Test]
    public void ToString_ShouldProvideReadableFormat()
    {
        // Arrange
        var packet = new AtemPacket([0x01, 0x02])
        {
            Flags = PacketFlag.AckRequest,
            SessionId = 0x1234,
            PacketId = 0x5678
        };

        // Act
        var result = packet.ToString();

        // Assert
        Assert.That(result, Does.Contain("AckRequest"));
        Assert.That(result, Does.Contain("4660")); // 0x1234 in decimal
        Assert.That(result, Does.Contain("22136")); // 0x5678 in decimal
        Assert.That(result, Does.Contain("14")); // Length = 12 + 2
    }

    [TestCase(PacketFlag.AckRequest, 0x01)]
    [TestCase(PacketFlag.NewSessionId, 0x02)]
    [TestCase(PacketFlag.IsRetransmit, 0x04)]
    [TestCase(PacketFlag.RetransmitRequest, 0x08)]
    [TestCase(PacketFlag.AckReply, 0x10)]
    public void PacketFlags_ShouldHaveCorrectValues(PacketFlag flag, byte expectedValue)
    {
        // Act & Assert
        Assert.That((byte)flag, Is.EqualTo(expectedValue));
    }

    [Test]
    public void MultipleFlags_ShouldCombineCorrectly()
    {
        // Arrange
        var combinedFlags = PacketFlag.AckRequest | PacketFlag.NewSessionId | PacketFlag.AckReply;

        // Act
        var packet = new AtemPacket([]) { Flags = combinedFlags };

        // Assert
        Assert.That(packet.HasFlag(PacketFlag.AckRequest), Is.True);
        Assert.That(packet.HasFlag(PacketFlag.NewSessionId), Is.True);
        Assert.That(packet.HasFlag(PacketFlag.AckReply), Is.True);
        Assert.That(packet.HasFlag(PacketFlag.IsRetransmit), Is.False);
        Assert.That(packet.HasFlag(PacketFlag.RetransmitRequest), Is.False);
    }
}
