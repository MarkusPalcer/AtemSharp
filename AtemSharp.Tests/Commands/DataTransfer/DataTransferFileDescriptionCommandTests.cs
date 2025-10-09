using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferFileDescriptionCommandTests : SerializedCommandTestBase<DataTransferFileDescriptionCommand,
    DataTransferFileDescriptionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string FileHash { get; set; } = string.Empty;
    }

    protected override DataTransferFileDescriptionCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the test data values
        var command = new DataTransferFileDescriptionCommand(
            testCase.Command.TransferId,
            testCase.Command.Name,
            testCase.Command.Description,
            testCase.Command.FileHash);

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
                Name = "testfile.txt",
                Description = "Test file description",
                FileHash = "FOZXZPX74a4paXCOTQHLCg==",
                Mask = 0xF // All properties set
            }
        };

        // Act
        var command = CreateSut(testCase);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(12345));
        Assert.That(command.Name, Is.EqualTo("testfile.txt"));
        Assert.That(command.Description, Is.EqualTo("Test file description"));
        Assert.That(command.FileHash, Is.EqualTo("FOZXZPX74a4paXCOTQHLCg=="));
    }

    [Test]
    public void DefaultConstructor_ShouldInitializeWithDefaults()
    {
        // Act
        var command = new DataTransferFileDescriptionCommand();

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0));
        Assert.That(command.Name, Is.Null);
        Assert.That(command.Description, Is.Null);
        Assert.That(command.FileHash, Is.EqualTo(string.Empty));
        Assert.That(command.Flag, Is.EqualTo(0));
    }

    [Test]
    public void SetProperties_ShouldUpdateFlagsCorrectly()
    {
        // Arrange
        var command = new DataTransferFileDescriptionCommand();

        // Act & Assert - Test each property sets its corresponding flag
        command.TransferId = 100;
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "TransferId should set flag bit 0");

        command.Name = "test.txt";
        Assert.That(command.Flag & (1 << 1), Is.Not.EqualTo(0), "Name should set flag bit 1");

        command.Description = "Test description";
        Assert.That(command.Flag & (1 << 2), Is.Not.EqualTo(0), "Description should set flag bit 2");

        command.FileHash = "dGVzdA==";
        Assert.That(command.Flag & (1 << 3), Is.Not.EqualTo(0), "FileHash should set flag bit 3");
    }

    [Test]
    public void Serialize_ShouldProduceCorrectByteArray()
    {
        // Arrange
        var command = new DataTransferFileDescriptionCommand(
            transferId: 0xEF4D, // 61005 in decimal 
            name: "591c580e-8ad3-4d94-a909-",
            description: "6616c138-75ea-435d-819c-b34564fc9fd89db5fec5-06f2-4b64-a93a-926254cf95d86a22f294-7d5",
            fileHash: "FOZXZPX74a4paXCOTQHLCg=="
        );

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.Length, Is.EqualTo(212), "Serialized command should be exactly 212 bytes");

        // Check transfer ID (first 2 bytes, big-endian)
        Assert.That(result[0], Is.EqualTo(0xEF), "First byte should be high byte of transfer ID");
        Assert.That(result[1], Is.EqualTo(0x4D), "Second byte should be low byte of transfer ID");

        // Check that name is written correctly (starts at byte 2)
        var nameBytes = System.Text.Encoding.UTF8.GetBytes("591c580e-8ad3-4d94-a909-");
        for (int i = 0; i < nameBytes.Length; i++)
        {
            Assert.That(result[2 + i], Is.EqualTo(nameBytes[i]), $"Name byte {i} should match");
        }

        // Check that description is written correctly (starts at byte 66)
        var descriptionBytes = System.Text.Encoding.UTF8.GetBytes("6616c138-75ea-435d-819c-b34564fc9fd89db5fec5-06f2-4b64-a93a-926254cf95d86a22f294-7d5");
        for (int i = 0; i < descriptionBytes.Length; i++)
        {
            Assert.That(result[66 + i], Is.EqualTo(descriptionBytes[i]), $"Description byte {i} should match");
        }

        // Check that file hash is written correctly (starts at byte 194)
        var expectedHashBytes = Convert.FromBase64String("FOZXZPX74a4paXCOTQHLCg==");
        for (int i = 0; i < expectedHashBytes.Length; i++)
        {
            Assert.That(result[194 + i], Is.EqualTo(expectedHashBytes[i]), $"File hash byte {i} should match");
        }
    }

    [Test]
    public void Serialize_WithNullStrings_ShouldHandleGracefully()
    {
        // Arrange
        var command = new DataTransferFileDescriptionCommand(
            transferId: 123,
            name: null,
            description: null,
            fileHash: "dGVzdA=="
        );

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.Length, Is.EqualTo(212));
        
        // Name section (bytes 2-65) should be all zeros
        for (int i = 2; i < 66; i++)
        {
            Assert.That(result[i], Is.EqualTo(0), $"Name byte {i - 2} should be zero when Name is null");
        }

        // Description section (bytes 66-193) should be all zeros
        for (int i = 66; i < 194; i++)
        {
            Assert.That(result[i], Is.EqualTo(0), $"Description byte {i - 66} should be zero when Description is null");
        }
    }

    [Test]
    public void Serialize_WithInvalidBase64Hash_ShouldHandleGracefully()
    {
        // Arrange
        var command = new DataTransferFileDescriptionCommand(
            transferId: 123,
            name: "test",
            description: "test desc",
            fileHash: "invalid-base64!@#"
        );

        // Act & Assert - Should not throw exception
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Hash section (bytes 194-209) should be all zeros due to invalid base64
        for (int i = 194; i < 210; i++)
        {
            Assert.That(result[i], Is.EqualTo(0), $"Hash byte {i - 194} should be zero when hash is invalid base64");
        }
    }

    [Test]
    public void Serialize_WithLongStrings_ShouldTruncateCorrectly()
    {
        // Arrange
        var longName = new string('A', 100); // 100 characters, should be truncated to 63
        var longDescription = new string('B', 200); // 200 characters, should be truncated to 127
        
        var command = new DataTransferFileDescriptionCommand(
            transferId: 123,
            name: longName,
            description: longDescription,
            fileHash: "dGVzdA=="
        );

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(result.Length, Is.EqualTo(212));

        // Check that name is truncated and null-terminated
        Assert.That(result[65], Is.EqualTo(0), "Name section should end with null terminator");
        for (int i = 2; i < 65; i++)
        {
            Assert.That(result[i], Is.EqualTo((byte)'A'), $"Name byte {i - 2} should be 'A'");
        }

        // Check that description is truncated and null-terminated
        Assert.That(result[193], Is.EqualTo(0), "Description section should end with null terminator");
        for (int i = 66; i < 193; i++)
        {
            Assert.That(result[i], Is.EqualTo((byte)'B'), $"Description byte {i - 66} should be 'B'");
        }
    }

    [Test]
    public void Serialize_WithEmptyFileHash_ShouldProduceZeroBytes()
    {
        // Arrange
        var command = new DataTransferFileDescriptionCommand(
            transferId: 123,
            name: "test",
            description: "test desc",
            fileHash: ""
        );

        // Act
        var result = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        // Hash section (bytes 194-209) should be all zeros
        for (int i = 194; i < 210; i++)
        {
            Assert.That(result[i], Is.EqualTo(0), $"Hash byte {i - 194} should be zero when hash is empty");
        }
    }
}