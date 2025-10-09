using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferDownloadRequestCommandTests : SerializedCommandTestBase<DataTransferDownloadRequestCommand,
    DataTransferDownloadRequestCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ushort TransferStoreId { get; set; }
        public ushort TransferIndex { get; set; }
        public ushort Unknown { get; set; }     // Maps to TransferType in our implementation
        public ushort Unknown2 { get; set; }    // Additional field not in TypeScript

        // Since FTSU commands don't use flags/masks, we'll default Mask to 0
        // The base class expects this property but FTSU test data doesn't have it
    }

    protected override DataTransferDownloadRequestCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the test data values
        var command = new DataTransferDownloadRequestCommand(
            testCase.Command.TransferId,
            testCase.Command.TransferStoreId,
            testCase.Command.TransferIndex,
            testCase.Command.Unknown,        // Maps to TransferType
            testCase.Command.Unknown2);

        return command;
    }

    // Additional manual tests for edge cases not covered by framework tests
    [Test]
    public void CreateSut_ShouldCorrectlyMapTestData()
    {
        // Arrange
        var testCase = new TestCaseData
        {
            Command = new CommandData
            {
                TransferId = 12345,
                TransferStoreId = 67,
                TransferIndex = 890,
                Unknown = 123,      // Maps to TransferType
                Unknown2 = 456,
                Mask = 0 // FTSU doesn't use masks
            }
        };

        // Act
        var command = CreateSut(testCase);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(12345));
        Assert.That(command.TransferStoreId, Is.EqualTo(67));
        Assert.That(command.TransferIndex, Is.EqualTo(890));
        Assert.That(command.TransferType, Is.EqualTo(123));
        Assert.That(command.Unknown2, Is.EqualTo(456));
    }
    
     [Test]
    public void Constructor_WithParameters_ShouldSetProperties()
    {
        // Arrange
        const ushort transferId = 23965;
        const ushort transferStoreId = 167;
        const ushort transferIndex = 4567;
        const ushort transferType = 89;
        const ushort unknown2 = 321;

        // Act
        var command = new DataTransferDownloadRequestCommand(transferId, transferStoreId, transferIndex, transferType, unknown2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(transferId));
        Assert.That(command.TransferStoreId, Is.EqualTo(transferStoreId));
        Assert.That(command.TransferIndex, Is.EqualTo(transferIndex));
        Assert.That(command.TransferType, Is.EqualTo(transferType));
        Assert.That(command.Unknown2, Is.EqualTo(unknown2));
    }

    [Test]
    public void Constructor_Default_ShouldInitializeWithZeroValues()
    {
        // Act
        var command = new DataTransferDownloadRequestCommand();

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0));
        Assert.That(command.TransferStoreId, Is.EqualTo(0));
        Assert.That(command.TransferIndex, Is.EqualTo(0));
        Assert.That(command.TransferType, Is.EqualTo(0));
        Assert.That(command.Unknown2, Is.EqualTo(0));
    }

    [Test]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var command = new DataTransferDownloadRequestCommand();

        // Act
        command.TransferId = 1234;
        command.TransferStoreId = 5678;
        command.TransferIndex = 9012;
        command.TransferType = 3456;
        command.Unknown2 = 7890;

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(1234));
        Assert.That(command.TransferStoreId, Is.EqualTo(5678));
        Assert.That(command.TransferIndex, Is.EqualTo(9012));
        Assert.That(command.TransferType, Is.EqualTo(3456));
        Assert.That(command.Unknown2, Is.EqualTo(7890));
    }

    [Test]
    public void Serialize_ShouldGenerateCorrectLength()
    {
        // Arrange
        var command = new DataTransferDownloadRequestCommand(1, 2, 3, 4, 5);

        // Act
        var serialized = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(serialized.Length, Is.EqualTo(12), "FTSU command payload should be exactly 12 bytes");
    }

    [Test]
    public void Serialize_ShouldPlaceValuesAtCorrectPositions()
    {
        // Arrange
        var command = new DataTransferDownloadRequestCommand(0x1234, 0x5678, 0x9ABC, 0xDEF0, 0x1122);

        // Act
        var serialized = command.Serialize(ProtocolVersion.V8_0);

        // Assert - verify the exact byte layout matching TypeScript implementation
        Assert.That(serialized.Length, Is.EqualTo(12));
        
        // TransferId at position 0-1 (big-endian)
        Assert.That(serialized[0], Is.EqualTo(0x12));
        Assert.That(serialized[1], Is.EqualTo(0x34));
        
        // TransferStoreId at position 2-3 (big-endian)
        Assert.That(serialized[2], Is.EqualTo(0x56));
        Assert.That(serialized[3], Is.EqualTo(0x78));
        
        // Padding at position 4-5
        Assert.That(serialized[4], Is.EqualTo(0x00));
        Assert.That(serialized[5], Is.EqualTo(0x00));
        
        // TransferIndex at position 6-7 (big-endian)
        Assert.That(serialized[6], Is.EqualTo(0x9A));
        Assert.That(serialized[7], Is.EqualTo(0xBC));
        
        // TransferType at position 8-9 (big-endian)
        Assert.That(serialized[8], Is.EqualTo(0xDE));
        Assert.That(serialized[9], Is.EqualTo(0xF0));
        
        // Unknown2 at position 10-11 (big-endian)
        Assert.That(serialized[10], Is.EqualTo(0x11));
        Assert.That(serialized[11], Is.EqualTo(0x22));
    }

    [Test]
    public void Serialize_WithZeroValues_ShouldGenerateCorrectBytes()
    {
        // Arrange
        var command = new DataTransferDownloadRequestCommand(0, 0, 0, 0, 0);

        // Act
        var serialized = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(serialized.Length, Is.EqualTo(12));
        Assert.That(serialized, Is.All.EqualTo(0), "All bytes should be zero when all properties are zero");
    }
}