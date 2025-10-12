using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands;

[TestFixture]
public class TimeConfigCommandTests : SerializedCommandTestBase<TimeConfigCommand, TimeConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public TimeMode Mode { get; set; }
    }

    protected override TimeConfigCommand CreateSut(TestCaseData testCase)
    {
        var command = new TimeConfigCommand(CreateMinimalStateForTesting());

        // Set the mode from test data (flag is set automatically)
        command.Mode = testCase.Command.Mode;

        return command;
    }

    private static AtemState CreateMinimalStateForTesting()
    {
        return new AtemState
        {
            Settings = new SettingsState
            {
                TimeMode = TimeMode.FreeRun  // Default value
            }
        };
    }

    [Test]
    public void Constructor_WithEmptyState_InitializesFromDefaults()
    {
        // Arrange
        var state = new AtemState(); // Empty state (but Settings is automatically initialized)

        // Act
        var command = new TimeConfigCommand(state);

        // Assert
        // Since Settings is always initialized in AtemState with default values,
        // the command should get the default TimeMode value (which is FreeRun = 0)
        Assert.That(command.Mode, Is.EqualTo(TimeMode.FreeRun));
        // No flag should be set since this is a BasicWritableCommand pattern
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set for BasicWritableCommand pattern");
    }

    [Test]
    public void Constructor_WithExistingState_InitializesFromState()
    {
        // Arrange
        var state = new AtemState
        {
            Settings = new SettingsState
            {
                TimeMode = TimeMode.TimeOfDay
            }
        };

        // Act
        var command = new TimeConfigCommand(state);

        // Assert
        Assert.That(command.Mode, Is.EqualTo(TimeMode.TimeOfDay));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set for BasicWritableCommand pattern");
    }

    [Test]
    public void Mode_WhenSet_DoesNotUpdateFlag()
    {
        // Arrange
        var state = CreateMinimalStateForTesting();
        var command = new TimeConfigCommand(state);
        
        // Act
        command.Mode = TimeMode.TimeOfDay;

        // Assert
        Assert.That(command.Mode, Is.EqualTo(TimeMode.TimeOfDay));
        Assert.That(command.Flag, Is.EqualTo(0), "Flag should not be set for BasicWritableCommand pattern");
    }
}