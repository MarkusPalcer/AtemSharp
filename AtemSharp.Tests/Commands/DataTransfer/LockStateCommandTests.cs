using AtemSharp.Commands.DataTransfer;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.DataTransfer;

[TestFixture]
public class LockStateCommandTests : SerializedCommandTestBase<LockStateCommand, LockStateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool Locked { get; set; }
    }

    protected override LockStateCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the test data values
        var command = new LockStateCommand(
            testCase.Command.Index,
            testCase.Command.Locked);

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
                Index = 12345,
                Locked = true,
                Mask = 0 // LOCK doesn't use masks
            }
        };

        // Act
        var command = CreateSut(testCase);

        // Assert
        Assert.That(command.Index, Is.EqualTo(12345));
        Assert.That(command.Locked, Is.True);
    }
    
    [Test]
    public void Constructor_WithParameters_ShouldSetProperties()
    {
        // Arrange
        const ushort index = 16928;
        const bool locked = true;

        // Act
        var command = new LockStateCommand(index, locked);

        // Assert
        Assert.That(command.Index, Is.EqualTo(index));
        Assert.That(command.Locked, Is.EqualTo(locked));
    }

    [Test]
    public void Serialize_ShouldProduceCorrectByteOrder()
    {
        // Arrange
        var command = new LockStateCommand(16928, true); // From test data
        
        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(4));
        
        // Index: 16928 (0x4220) -> 0x42, 0x20 in big-endian
        Assert.That(result[0], Is.EqualTo(0x42), "First byte should be high byte of Index");
        Assert.That(result[1], Is.EqualTo(0x20), "Second byte should be low byte of Index");
        
        // Locked: true -> 0x01
        Assert.That(result[2], Is.EqualTo(0x01), "Third byte should be 1 for true");
        
        // Padding
        Assert.That(result[3], Is.EqualTo(0x00), "Fourth byte should be padding (0)");
    }

    [Test]
    public void Serialize_WithLockedFalse_ShouldProduceCorrectByteOrder()
    {
        // Arrange
        var command = new LockStateCommand(55773, false); // From test data
        
        // Act
        var result = command.Serialize(ProtocolVersion.V8_1_1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(4));
        
        // Index: 55773 (0xD9DD) -> 0xD9, 0xDD in big-endian  
        Assert.That(result[0], Is.EqualTo(0xD9), "First byte should be high byte of Index");
        Assert.That(result[1], Is.EqualTo(0xDD), "Second byte should be low byte of Index");
        
        // Locked: false -> 0x00
        Assert.That(result[2], Is.EqualTo(0x00), "Third byte should be 0 for false");
        
        // Padding
        Assert.That(result[3], Is.EqualTo(0x00), "Fourth byte should be padding (0)");
    }
}