using AtemSharp.Commands.DisplayClock;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.DisplayClock;

[TestFixture]
public class DisplayClockRequestTimeCommandTests : SerializedCommandTestBase<DisplayClockRequestTimeCommand,
    DisplayClockRequestTimeCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        // This command has no properties
    }

    protected override DisplayClockRequestTimeCommand CreateSut(TestCaseData testCase)
    {
        // Create command (no parameters needed)
        return new DisplayClockRequestTimeCommand();
    }

    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Act
        var command = new DisplayClockRequestTimeCommand();

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0)); // No flags for this command
    }

    [Test]
    public void Serialize_ShouldProduceCorrectLength()
    {
        // Arrange
        var command = new DisplayClockRequestTimeCommand();

        // Act
        var serialized = command.Serialize(ProtocolVersion.V8_0);

        // Assert
        Assert.That(serialized.Length, Is.EqualTo(4));
        Assert.That(serialized, Is.All.EqualTo(0)); // Should be all zeros (padding)
    }
}
