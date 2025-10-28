using AtemSharp.Commands.DisplayClock;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.DisplayClock;

[TestFixture]
public class DisplayClockStateSetCommandTests : SerializedCommandTestBase<DisplayClockStateSetCommand,
    DisplayClockStateSetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public DisplayClockClockState State { get; set; }
    }

    protected override DisplayClockStateSetCommand CreateSut(TestCaseData testCase)
    {
        // Create command with the state from test data
        var command = new DisplayClockStateSetCommand(new AtemSharp.State.DisplayClock { ClockState = testCase.Command.State });
        return command;
    }

    [Test]
    public void Constructor_ShouldSetStateCorrectly()
    {
        // Arrange & Act
        var command = new DisplayClockStateSetCommand(new AtemSharp.State.DisplayClock { ClockState = DisplayClockClockState.Running });

        // Assert
        Assert.That(command.State, Is.EqualTo(DisplayClockClockState.Running));
        Assert.That(command.Flag, Is.EqualTo(0)); // Flag should not be set
    }

    [TestCase(DisplayClockClockState.Stopped)]
    [TestCase(DisplayClockClockState.Running)]
    [TestCase(DisplayClockClockState.Reset)]
    public void State_ShouldAcceptAllValidValues(DisplayClockClockState state)
    {
        // Arrange
        var command = new DisplayClockStateSetCommand(new AtemSharp.State.DisplayClock { ClockState = DisplayClockClockState.Stopped });

        // Act
        command.State = state;

        // Assert
        Assert.That(command.State, Is.EqualTo(state));
        Assert.That(command.Flag, Is.EqualTo(1)); // Flag should be set
    }
}
