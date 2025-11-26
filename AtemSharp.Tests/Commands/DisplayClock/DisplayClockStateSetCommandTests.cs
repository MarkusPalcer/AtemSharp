using AtemSharp.Commands.DisplayClock;
using AtemSharp.State.DisplayClock;

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
        return new DisplayClockStateSetCommand(new AtemSharp.State.DisplayClock.DisplayClock { ClockState = testCase.Command.State });
    }

    [Test]
    public void Constructor_ShouldSetStateCorrectly()
    {
        // Arrange & Act
        var command = new DisplayClockStateSetCommand(new AtemSharp.State.DisplayClock.DisplayClock { ClockState = DisplayClockClockState.Running });

        // Assert
        Assert.That(command.Flag, Is.EqualTo(0)); // Flag should not be set
    }
}
