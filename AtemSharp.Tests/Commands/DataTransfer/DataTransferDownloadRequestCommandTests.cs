using AtemSharp.Commands.DataTransfer;

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
        var command = new DataTransferDownloadRequestCommand
        {
            TransferId = testCase.Command.TransferId,
            TransferStoreId = testCase.Command.TransferStoreId,
            TransferIndex = testCase.Command.TransferIndex,
            TransferType = testCase.Command.Unknown,
            Unknown2 = testCase.Command.Unknown2,
        };

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
        var command = new DataTransferDownloadRequestCommand
        {
            TransferId = transferId,
            TransferStoreId = transferStoreId,
            TransferIndex = transferIndex,
            TransferType = transferType,
            Unknown2 = unknown2
        };

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

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(1234));
        Assert.That(command.TransferStoreId, Is.EqualTo(5678));
        Assert.That(command.TransferIndex, Is.EqualTo(9012));
        Assert.That(command.TransferType, Is.EqualTo(3456));
    }
}
