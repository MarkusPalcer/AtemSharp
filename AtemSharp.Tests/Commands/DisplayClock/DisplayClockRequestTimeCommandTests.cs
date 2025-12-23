using AtemSharp.Commands.DisplayClock;

namespace AtemSharp.Tests.Commands.DisplayClock;

[TestFixture]
public class DisplayClockRequestTimeCommandTests : SerializedCommandTestBase<DisplayClockRequestTimeCommand,
    DisplayClockRequestTimeCommandTests.CommandData>
{
    public class CommandData : CommandDataBase;

    protected override DisplayClockRequestTimeCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new DisplayClockRequestTimeCommand();
    }

    [Test]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Act
        var command = new DisplayClockRequestTimeCommand();

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0));
    }
}
