using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferUploadContinueCommandTests : DeserializedCommandTestBase<DataTransferUploadContinueCommand, DataTransferUploadContinueCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ushort ChunkSize { get; set; }
        public ushort ChunkCount { get; set; }
    }

    protected override void CompareCommandProperties(DataTransferUploadContinueCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare TransferId - it is not floating point so it needs to equal exactly
        if (!actualCommand.TransferId.Equals(expectedData.TransferId))
        {
            failures.Add($"TransferId: expected {expectedData.TransferId}, actual {actualCommand.TransferId}");
        }

        // Compare ChunkSize - it is not floating point so it needs to equal exactly
        if (!actualCommand.ChunkSize.Equals(expectedData.ChunkSize))
        {
            failures.Add($"ChunkSize: expected {expectedData.ChunkSize}, actual {actualCommand.ChunkSize}");
        }

        // Compare ChunkCount - it is not floating point so it needs to equal exactly
        if (!actualCommand.ChunkCount.Equals(expectedData.ChunkCount))
        {
            failures.Add($"ChunkCount: expected {expectedData.ChunkCount}, actual {actualCommand.ChunkCount}");
        }

        // Assert results
        if (failures.Count > 0)
        {
            Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
                       string.Join("\n", failures));
        }
    }

    [Test]
    public void ApplyToState_ShouldReturnEmptyArray()
    {
        // Arrange
        var command = new DataTransferUploadContinueCommand
        {
            TransferId = 12345,
            ChunkSize = 1024,
            ChunkCount = 5
        };
        var state = new State.AtemState();

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty, "DataTransferUploadContinueCommand should not modify state and return empty array");
    }

    [Test]
    public void Deserialize_ShouldCorrectlyParseAllFields()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort expectedTransferId = 0x1234; // 4660 in decimal
        const ushort expectedChunkSize = 0x0400;  // 1024 in decimal
        const ushort expectedChunkCount = 0x0005; // 5 in decimal
        
        writer.Write((byte)0x12); // TransferId high byte (offset 0)
        writer.Write((byte)0x34); // TransferId low byte (offset 1)
        writer.Write((byte)0x00); // Padding (offset 2)
        writer.Write((byte)0x00); // Padding (offset 3)
        writer.Write((byte)0x00); // Padding (offset 4)
        writer.Write((byte)0x00); // Padding (offset 5)
        writer.Write((byte)0x04); // ChunkSize high byte (offset 6)
        writer.Write((byte)0x00); // ChunkSize low byte (offset 7)
        writer.Write((byte)0x00); // ChunkCount high byte (offset 8)
        writer.Write((byte)0x05); // ChunkCount low byte (offset 9)
        
        stream.Position = 0;

        // Act
        var command = DataTransferUploadContinueCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(expectedTransferId));
        Assert.That(command.ChunkSize, Is.EqualTo(expectedChunkSize));
        Assert.That(command.ChunkCount, Is.EqualTo(expectedChunkCount));
    }

    [Test]
    public void Deserialize_ShouldHandleZeroValues()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        // Write all zeros
        writer.Write(new byte[10]);
        
        stream.Position = 0;

        // Act
        var command = DataTransferUploadContinueCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0));
        Assert.That(command.ChunkSize, Is.EqualTo(0));
        Assert.That(command.ChunkCount, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_ShouldHandleMaxValues()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort maxValue = 0xFFFF; // 65535 in decimal
        
        writer.Write((byte)0xFF); // TransferId high byte
        writer.Write((byte)0xFF); // TransferId low byte
        writer.Write((byte)0x00); // Padding
        writer.Write((byte)0x00); // Padding  
        writer.Write((byte)0x00); // Padding
        writer.Write((byte)0x00); // Padding
        writer.Write((byte)0xFF); // ChunkSize high byte
        writer.Write((byte)0xFF); // ChunkSize low byte
        writer.Write((byte)0xFF); // ChunkCount high byte
        writer.Write((byte)0xFF); // ChunkCount low byte
        
        stream.Position = 0;

        // Act
        var command = DataTransferUploadContinueCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(maxValue));
        Assert.That(command.ChunkSize, Is.EqualTo(maxValue));
        Assert.That(command.ChunkCount, Is.EqualTo(maxValue));
    }

    [Test]
    public void Deserialize_ShouldHandleTypicalUploadContinueValues()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort transferId = 42;
        const ushort chunkSize = 1416; // Typical ATEM packet size  
        const ushort chunkCount = 10;
        
        writer.Write((byte)0x00); // TransferId high byte (42 = 0x002A)
        writer.Write((byte)0x2A); // TransferId low byte
        writer.Write((byte)0x00); // Padding
        writer.Write((byte)0x00); // Padding
        writer.Write((byte)0x00); // Padding
        writer.Write((byte)0x00); // Padding
        writer.Write((byte)0x05); // ChunkSize high byte (1416 = 0x0588)
        writer.Write((byte)0x88); // ChunkSize low byte
        writer.Write((byte)0x00); // ChunkCount high byte (10 = 0x000A)
        writer.Write((byte)0x0A); // ChunkCount low byte
        
        stream.Position = 0;

        // Act
        var command = DataTransferUploadContinueCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(transferId));
        Assert.That(command.ChunkSize, Is.EqualTo(chunkSize));
        Assert.That(command.ChunkCount, Is.EqualTo(chunkCount));
    }

    [Test]
    public void Deserialize_ShouldIgnorePaddingBytes()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort expectedTransferId = 0x1111;
        const ushort expectedChunkSize = 0x2222;
        const ushort expectedChunkCount = 0x3333;
        
        writer.Write((byte)0x11); // TransferId high byte
        writer.Write((byte)0x11); // TransferId low byte
        writer.Write((byte)0xAA); // Padding - should be ignored
        writer.Write((byte)0xBB); // Padding - should be ignored
        writer.Write((byte)0xCC); // Padding - should be ignored
        writer.Write((byte)0xDD); // Padding - should be ignored
        writer.Write((byte)0x22); // ChunkSize high byte
        writer.Write((byte)0x22); // ChunkSize low byte
        writer.Write((byte)0x33); // ChunkCount high byte
        writer.Write((byte)0x33); // ChunkCount low byte
        
        stream.Position = 0;

        // Act
        var command = DataTransferUploadContinueCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert - padding bytes should not affect the results
        Assert.That(command.TransferId, Is.EqualTo(expectedTransferId));
        Assert.That(command.ChunkSize, Is.EqualTo(expectedChunkSize));
        Assert.That(command.ChunkCount, Is.EqualTo(expectedChunkCount));
    }
}