using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferAckCommandTests : SerializedCommandTestBase<DataTransferAckCommand,
    DataTransferAckCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public byte TransferIndex { get; set; }

        // Since FTUA commands don't use flags/masks, we'll default Mask to 0
        // The base class expects this property but FTUA test data doesn't have it
    }

    protected override DataTransferAckCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the test data values
        var command = new DataTransferAckCommand(
            testCase.Command.TransferId,
            testCase.Command.TransferIndex);

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
                TransferIndex = 67,
                Mask = 0 // FTUA doesn't use masks
            }
        };

        // Act
        var command = CreateSut(testCase);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(12345));
        Assert.That(command.TransferIndex, Is.EqualTo(67));
    }
    
     [Test]
    public void Constructor_WithParameters_ShouldSetProperties()
    {
        // Arrange
        const ushort transferId = 23965;
        const byte transferIndex = 167;

        // Act
        var command = new DataTransferAckCommand(transferId, transferIndex);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(transferId));
        Assert.That(command.TransferIndex, Is.EqualTo(transferIndex));
    }

    [Test]
    public void Constructor_Default_ShouldInitializeWithZeroValues()
    {
        // Act
        var command = new DataTransferAckCommand();

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0));
        Assert.That(command.TransferIndex, Is.EqualTo(0));
    }

    [Test]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var command = new DataTransferAckCommand();
        const ushort transferId = 57390;
        const byte transferIndex = 9;

        // Act
        command.TransferId = transferId;
        command.TransferIndex = transferIndex;

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(transferId));
        Assert.That(command.TransferIndex, Is.EqualTo(transferIndex));
    }

    [Test]
    public void Serialize_WithTestData1_ShouldMatchExpectedBytes()
    {
        // Arrange - From libatem-data.json test case
        var command = new DataTransferAckCommand(23965, 167);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert - Expected: 5D-9D-A7-00 (transferId: 23965, transferIndex: 167, padding: 0)
        var expected = new byte[] { 0x5D, 0x9D, 0xA7, 0x00 };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Serialize_WithTestData2_ShouldMatchExpectedBytes()
    {
        // Arrange - From libatem-data.json test case
        var command = new DataTransferAckCommand(37029, 237);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert - Expected: 90-A5-ED-00 (transferId: 37029, transferIndex: 237, padding: 0)
        var expected = new byte[] { 0x90, 0xA5, 0xED, 0x00 };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Serialize_WithTestData3_ShouldMatchExpectedBytes()
    {
        // Arrange - From libatem-data.json test case
        var command = new DataTransferAckCommand(57390, 9);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert - Expected: E0-2E-09-00 (transferId: 57390, transferIndex: 9, padding: 0)
        var expected = new byte[] { 0xE0, 0x2E, 0x09, 0x00 };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Serialize_WithZeroValues_ShouldProduceZeroBytes()
    {
        // Arrange
        var command = new DataTransferAckCommand(0, 0);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert - Expected: 00-00-00-00
        var expected = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Serialize_WithMaxValues_ShouldHandleMaxValues()
    {
        // Arrange
        var command = new DataTransferAckCommand(ushort.MaxValue, byte.MaxValue);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert - Expected: FF-FF-FF-00 (maxUShort: 65535, maxByte: 255, padding: 0)
        var expected = new byte[] { 0xFF, 0xFF, 0xFF, 0x00 };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Serialize_ShouldProduceCorrectLength()
    {
        // Arrange
        var command = new DataTransferAckCommand(1234, 56);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result.Length, Is.EqualTo(4));
    }

    [Test]
    public void Serialize_ShouldBeBigEndian()
    {
        // Arrange - Use a value where endianness is obvious
        var command = new DataTransferAckCommand(0x1234, 0x56);

        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert - Big-endian: 0x12 then 0x34, then 0x56, then padding 0x00
        Assert.That(result[0], Is.EqualTo(0x12));
        Assert.That(result[1], Is.EqualTo(0x34));
        Assert.That(result[2], Is.EqualTo(0x56));
        Assert.That(result[3], Is.EqualTo(0x00));
    }
}