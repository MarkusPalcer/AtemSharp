using AtemSharp.Commands.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferCompleteCommandTests : DeserializedCommandTestBase<DataTransferCompleteCommand, DataTransferCompleteCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
    }

    protected override void CompareCommandProperties(DataTransferCompleteCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare TransferId - it is not floating point so it needs to equal exactly
        if (!actualCommand.TransferId.Equals(expectedData.TransferId))
        {
            failures.Add($"TransferId: expected {expectedData.TransferId}, actual {actualCommand.TransferId}");
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
        var command = new DataTransferCompleteCommand
        {
            TransferId = 12345
        };
        var state = new State.AtemState();

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty, "DataTransferCompleteCommand should not modify state and return empty array");
    }

    [Test]
    public void Deserialize_ShouldCorrectlyParseTransferId()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort expectedTransferId = 0x1234; // 4660 in decimal
        writer.Write((byte)0x12); // High byte first (big-endian)
        writer.Write((byte)0x34); // Low byte
        
        stream.Position = 0;

        // Act
        var command = DataTransferCompleteCommand.Deserialize(stream);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(expectedTransferId));
    }

    [Test]
    public void Deserialize_ShouldHandleZeroTransferId()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        writer.Write((byte)0x00);
        writer.Write((byte)0x00);
        
        stream.Position = 0;

        // Act
        var command = DataTransferCompleteCommand.Deserialize(stream);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_ShouldHandleMaxTransferId()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort maxTransferId = 0xFFFF; // 65535 in decimal
        writer.Write((byte)0xFF);
        writer.Write((byte)0xFF);
        
        stream.Position = 0;

        // Act
        var command = DataTransferCompleteCommand.Deserialize(stream);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(maxTransferId));
    }
}