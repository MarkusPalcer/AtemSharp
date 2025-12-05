using AtemSharp.Commands;

namespace AtemSharp.Tests.Lib;

[TestFixture]
public class DeserializationExtensionsTests
{
    [Test]
    public void ReadUInt16BigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        ReadOnlySpan<byte> span = data;

        // Act
        var value1 = span.ReadUInt16BigEndian(0);
        var value2 = span.ReadUInt16BigEndian(2);

        // Assert
        Assert.That(value1, Is.EqualTo(0x1234));
        Assert.That(value2, Is.EqualTo(0x5678));
    }

    [Test]
    public void ReadUInt32BigEndian_ShouldReadCorrectValue()
    {
        // Arrange
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78, 0xAB, 0xCD, 0xEF, 0x00 };
        ReadOnlySpan<byte> span = data;

        // Act
        var value1 = span.ReadUInt32BigEndian(0);
        var value2 = span.ReadUInt32BigEndian(4);

        // Assert
        Assert.That(value1, Is.EqualTo(0x12345678U));
        Assert.That(value2, Is.EqualTo(0xABCDEF00U));
    }

    [Test]
    public void ReadInt16BigEndian_ShouldReadSignedValues()
    {
        // Arrange
        var data = new byte[] { 0x80, 0x00, 0x7F, 0xFF }; // -32768, 32767
        ReadOnlySpan<byte> span = data;

        // Act
        var value1 = span.ReadInt16BigEndian(0);
        var value2 = span.ReadInt16BigEndian(2);

        // Assert
        Assert.That(value1, Is.EqualTo(-32768));
        Assert.That(value2, Is.EqualTo(32767));
    }

    [Test]
    public void ToMemoryStream_ShouldCreateStreamWithCorrectData()
    {
        // Arrange
        var data = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        ReadOnlySpan<byte> span = data;

        // Act
        using var stream = span.ToMemoryStream();
        using var reader = new BinaryReader(stream);

        // Assert
        Assert.That(stream.Length, Is.EqualTo(4));
        Assert.That(reader.ReadByte(), Is.EqualTo(0x01));
        Assert.That(reader.ReadByte(), Is.EqualTo(0x02));
        Assert.That(reader.ReadByte(), Is.EqualTo(0x03));
        Assert.That(reader.ReadByte(), Is.EqualTo(0x04));
    }

    [Test]
    public void RoundTripConversion_ShouldPreserveValues()
    {
        // Arrange
        var buffer = new byte[12];
        var writeSpan = buffer.AsSpan();

        const ushort testUShort = 0x1234;
        const uint testUInt = 0xABCDEF00;
        const short testShort = -12345;

        // Act - Write values
        buffer.WriteUInt16BigEndian(testUShort, 0);
        buffer.WriteUInt32BigEndian(testUInt, 2);
        buffer.WriteInt16BigEndian(testShort, 6);

        // Act - Read values back
        ReadOnlySpan<byte> readSpan = buffer;
        var readUShort = readSpan.ReadUInt16BigEndian(0);
        var readUInt = readSpan.ReadUInt32BigEndian(2);
        var readShort = readSpan.ReadInt16BigEndian(6);

        // Assert
        Assert.That(readUShort, Is.EqualTo(testUShort));
        Assert.That(readUInt, Is.EqualTo(testUInt));
        Assert.That(readShort, Is.EqualTo(testShort));
    }
}
