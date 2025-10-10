using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class LockObtainedCommandTests : DeserializedCommandTestBase<LockObtainedCommand, LockObtainedCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
    }

    protected override void CompareCommandProperties(LockObtainedCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Index - it is not floating point so it needs to equal exactly
        if (!actualCommand.Index.Equals(expectedData.Index))
        {
            failures.Add($"Index: expected {expectedData.Index}, actual {actualCommand.Index}");
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
        var command = new LockObtainedCommand
        {
            Index = 12345
        };
        var state = new State.AtemState();

        // Act
        var result = command.ApplyToState(state);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty, "LockObtainedCommand should not modify state and return empty array");
    }

    [Test]
    public void Deserialize_ShouldCorrectlyParseIndex()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort expectedIndex = 0x1234; // 4660 in decimal
        writer.Write((byte)0x12); // High byte first (big-endian)
        writer.Write((byte)0x34); // Low byte
        
        stream.Position = 0;

        // Act
        var command = LockObtainedCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(expectedIndex));
    }

    [Test]
    public void Deserialize_ShouldHandleZeroIndex()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        writer.Write((byte)0x00);
        writer.Write((byte)0x00);
        
        stream.Position = 0;

        // Act
        var command = LockObtainedCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(0));
    }

    [Test]
    public void Deserialize_ShouldHandleMaxIndex()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        
        const ushort maxIndex = 0xFFFF; // 65535 in decimal
        writer.Write((byte)0xFF);
        writer.Write((byte)0xFF);
        
        stream.Position = 0;

        // Act
        var command = LockObtainedCommand.Deserialize(stream, ProtocolVersion.V7_2);

        // Assert
        Assert.That(command.Index, Is.EqualTo(maxIndex));
    }

    [Test]
    public void Deserialize_ShouldHandleTypicalLockIndices()
    {
        // Test some typical lock indices based on TypeScript implementation comments
        var testCases = new[]
        {
            (byte1: (byte)0x00, byte2: (byte)0x00, expected: (ushort)0), // Stills lock
            (byte1: (byte)0x00, byte2: (byte)0x01, expected: (ushort)1), // Clip lock 1
            (byte1: (byte)0x00, byte2: (byte)0x02, expected: (ushort)2), // Clip lock 2
            (byte1: (byte)0x00, byte2: (byte)0x64, expected: (ushort)100), // Special lock boundary
        };

        foreach (var (byte1, byte2, expected) in testCases)
        {
            // Arrange
            using var stream = new MemoryStream();
            using var writer = new BinaryWriter(stream);
            
            writer.Write(byte1);
            writer.Write(byte2);
            
            stream.Position = 0;

            // Act
            var command = LockObtainedCommand.Deserialize(stream, ProtocolVersion.V7_2);

            // Assert
            Assert.That(command.Index, Is.EqualTo(expected), $"Failed for bytes 0x{byte1:X2}{byte2:X2}");
        }
    }
}