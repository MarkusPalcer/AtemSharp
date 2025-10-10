using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;
using AtemSharp.Enums.DataTransfer;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class DataTransferErrorCommandTests : DeserializedCommandTestBase<DataTransferErrorCommand, DataTransferErrorCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort TransferId { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }

    protected override void CompareCommandProperties(DataTransferErrorCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare TransferId - it is not floating point so it needs to equal exactly
        if (!actualCommand.TransferId.Equals(expectedData.TransferId))
        {
            failures.Add($"TransferId: expected {expectedData.TransferId}, actual {actualCommand.TransferId}");
        }

        // Compare ErrorCode - enum values should match exactly
        if (!actualCommand.ErrorCode.Equals(expectedData.ErrorCode))
        {
            failures.Add($"ErrorCode: expected {expectedData.ErrorCode}, actual {actualCommand.ErrorCode}");
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
        var command = new DataTransferErrorCommand
        {
            TransferId = 12345,
            ErrorCode = ErrorCode.Retry
        };
        var state = new State.AtemState();

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty, "DataTransferErrorCommand should not modify state and return empty array");
    }

    [Test]
    public void Deserialize_ShouldCorrectlyParseTransferIdAndErrorCode()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort expectedTransferId = 0x1234; // 4660 in decimal
        const ErrorCode expectedErrorCode = ErrorCode.NotFound;
        
        writer.Write((byte)0x12); // High byte first (big-endian)
        writer.Write((byte)0x34); // Low byte
        writer.Write((byte)expectedErrorCode); // Error code as byte
        
        stream.Position = 0;

        // Act
        var command = DataTransferErrorCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(expectedTransferId));
        Assert.That(command.ErrorCode, Is.EqualTo(expectedErrorCode));
    }

    [Test]
    public void Deserialize_ShouldHandleZeroTransferId()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        writer.Write((byte)0x00);
        writer.Write((byte)0x00);
        writer.Write((byte)ErrorCode.Retry);
        
        stream.Position = 0;

        // Act
        var command = DataTransferErrorCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0));
        Assert.That(command.ErrorCode, Is.EqualTo(ErrorCode.Retry));
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
        writer.Write((byte)ErrorCode.NotLocked);
        
        stream.Position = 0;

        // Act
        var command = DataTransferErrorCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(maxTransferId));
        Assert.That(command.ErrorCode, Is.EqualTo(ErrorCode.NotLocked));
    }

    [Test]
    [TestCase(ErrorCode.Retry, (byte)1)]
    [TestCase(ErrorCode.NotFound, (byte)2)]
    [TestCase(ErrorCode.NotLocked, (byte)5)]
    public void Deserialize_ShouldHandleAllErrorCodes(ErrorCode expectedErrorCode, byte errorCodeByte)
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort transferId = 0x1000;
        writer.Write((byte)0x10);
        writer.Write((byte)0x00);
        writer.Write(errorCodeByte);
        
        stream.Position = 0;

        // Act
        var command = DataTransferErrorCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(transferId));
        Assert.That(command.ErrorCode, Is.EqualTo(expectedErrorCode));
    }

    [Test]
    public void Deserialize_ShouldHandleUnknownErrorCode()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort transferId = 0x1000;
        const byte unknownErrorCode = 99; // Unknown error code
        
        writer.Write((byte)0x10);
        writer.Write((byte)0x00);
        writer.Write(unknownErrorCode);
        
        stream.Position = 0;

        // Act
        var command = DataTransferErrorCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(transferId));
        Assert.That((byte)command.ErrorCode, Is.EqualTo(unknownErrorCode), 
                   "Unknown error codes should be preserved as their byte value");
    }

    [Test]
    public void Deserialize_ShouldPreserveBigEndianByteOrder()
    {
        // Arrange - Test with specific byte values to ensure big-endian parsing
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        // Write bytes in big-endian order: 0xABCD should result in decimal 43981
        writer.Write((byte)0xAB); // High byte
        writer.Write((byte)0xCD); // Low byte
        writer.Write((byte)ErrorCode.NotFound);
        
        stream.Position = 0;

        // Act
        var command = DataTransferErrorCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.TransferId, Is.EqualTo(0xABCD));
        Assert.That(command.ErrorCode, Is.EqualTo(ErrorCode.NotFound));
    }
}