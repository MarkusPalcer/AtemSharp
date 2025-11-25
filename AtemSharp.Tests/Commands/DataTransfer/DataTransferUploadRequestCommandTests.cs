using AtemSharp.Commands.DataTransfer;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferUploadRequestCommandTests : SerializedCommandTestBase<DataTransferUploadRequestCommand,
    DataTransferUploadRequestCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ushort TransferStoreId { get; set; }
        public ushort TransferIndex { get; set; }
        public uint Size { get; set; }
        public ushort Mode { get; set; }

        // Since FTSD commands don't use flags/masks, we'll default Mask to 0
        // The base class expects this property but FTSD test data doesn't have it
    }

    protected override DataTransferUploadRequestCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the test data values
        var command = new DataTransferUploadRequestCommand(
            testCase.Command.TransferId,
            testCase.Command.TransferStoreId,
            testCase.Command.TransferIndex,
            testCase.Command.Size,
            testCase.Command.Mode);

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
                Size = 1024576,
                Mode = 256,
                Mask = 0 // FTSD doesn't use masks
            }
        };

        // Act
        var command = CreateSut(testCase);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(12345));
        Assert.That(command.TransferStoreId, Is.EqualTo(67));
        Assert.That(command.TransferIndex, Is.EqualTo(890));
        Assert.That(command.Size, Is.EqualTo(1024576));
        Assert.That(command.Mode, Is.EqualTo(256));
    }

    [Test]
    public void Constructor_WithParameters_ShouldSetProperties()
    {
        // Arrange
        const ushort transferId = 23965;
        const ushort transferStoreId = 167;
        const ushort transferIndex = 4567;
        const uint size = 2048000;
        const ushort mode = 768;

        // Act
        var command = new DataTransferUploadRequestCommand(transferId, transferStoreId, transferIndex, size, mode);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(transferId));
        Assert.That(command.TransferStoreId, Is.EqualTo(transferStoreId));
        Assert.That(command.TransferIndex, Is.EqualTo(transferIndex));
        Assert.That(command.Size, Is.EqualTo(size));
        Assert.That(command.Mode, Is.EqualTo(mode));
    }

    [Test]
    public void Constructor_Default_ShouldSetPropertiesToZero()
    {
        // Act
        var command = new DataTransferUploadRequestCommand();

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0));
        Assert.That(command.TransferStoreId, Is.EqualTo(0));
        Assert.That(command.TransferIndex, Is.EqualTo(0));
        Assert.That(command.Size, Is.EqualTo(0));
        Assert.That(command.Mode, Is.EqualTo(0));
    }

    [Test]
    public void Serialize_ShouldProduceCorrectByteOrder()
    {
        // Arrange
        var command = new DataTransferUploadRequestCommand(0x1234, 0x5678, 0xABCD, 0x12345678, 0xEF01);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.Length, Is.EqualTo(16));

        // Check each field is serialized in big-endian format
        Assert.That(result[0], Is.EqualTo(0x12)); // TransferId high byte
        Assert.That(result[1], Is.EqualTo(0x34)); // TransferId low byte
        Assert.That(result[2], Is.EqualTo(0x56)); // TransferStoreId high byte
        Assert.That(result[3], Is.EqualTo(0x78)); // TransferStoreId low byte
        Assert.That(result[4], Is.EqualTo(0x00)); // Padding
        Assert.That(result[5], Is.EqualTo(0x00)); // Padding
        Assert.That(result[6], Is.EqualTo(0xAB)); // TransferIndex high byte
        Assert.That(result[7], Is.EqualTo(0xCD)); // TransferIndex low byte
        Assert.That(result[8], Is.EqualTo(0x12)); // Size byte 0 (most significant)
        Assert.That(result[9], Is.EqualTo(0x34)); // Size byte 1
        Assert.That(result[10], Is.EqualTo(0x56)); // Size byte 2
        Assert.That(result[11], Is.EqualTo(0x78)); // Size byte 3 (least significant)
        Assert.That(result[12], Is.EqualTo(0xEF)); // Mode high byte
        Assert.That(result[13], Is.EqualTo(0x01)); // Mode low byte
        Assert.That(result[14], Is.EqualTo(0x00)); // Padding
        Assert.That(result[15], Is.EqualTo(0x00)); // Padding
    }

    [Test]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var command = new DataTransferUploadRequestCommand();

        // Act
        command.TransferId = 999;
        command.TransferStoreId = 111;
        command.TransferIndex = 222;
        command.Size = 333444;
        command.Mode = 555;

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(999));
        Assert.That(command.TransferStoreId, Is.EqualTo(111));
        Assert.That(command.TransferIndex, Is.EqualTo(222));
        Assert.That(command.Size, Is.EqualTo(333444));
        Assert.That(command.Mode, Is.EqualTo(555));
    }

    [Test]
    public void Serialize_WithMinValues_ShouldWork()
    {
        // Arrange
        var command = new DataTransferUploadRequestCommand(0, 0, 0, 0, 0);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.Length, Is.EqualTo(16));
        Assert.That(result, Is.All.EqualTo(0)); // All bytes should be zero
    }

    [Test]
    public void Serialize_WithMaxValues_ShouldWork()
    {
        // Arrange
        var command = new DataTransferUploadRequestCommand(0xFFFF, 0xFFFF, 0xFFFF, 0xFFFFFFFF, 0xFFFF);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.Length, Is.EqualTo(16));

        // Check that max values are properly serialized (except padding bytes)
        Assert.That(result[0], Is.EqualTo(0xFF)); // TransferId high
        Assert.That(result[1], Is.EqualTo(0xFF)); // TransferId low
        Assert.That(result[2], Is.EqualTo(0xFF)); // TransferStoreId high
        Assert.That(result[3], Is.EqualTo(0xFF)); // TransferStoreId low
        Assert.That(result[4], Is.EqualTo(0x00)); // Padding
        Assert.That(result[5], Is.EqualTo(0x00)); // Padding
        Assert.That(result[6], Is.EqualTo(0xFF)); // TransferIndex high
        Assert.That(result[7], Is.EqualTo(0xFF)); // TransferIndex low
        Assert.That(result[8], Is.EqualTo(0xFF)); // Size byte 0
        Assert.That(result[9], Is.EqualTo(0xFF)); // Size byte 1
        Assert.That(result[10], Is.EqualTo(0xFF)); // Size byte 2
        Assert.That(result[11], Is.EqualTo(0xFF)); // Size byte 3
        Assert.That(result[12], Is.EqualTo(0xFF)); // Mode high
        Assert.That(result[13], Is.EqualTo(0xFF)); // Mode low
        Assert.That(result[14], Is.EqualTo(0x00)); // Padding
        Assert.That(result[15], Is.EqualTo(0x00)); // Padding
    }
}
