using AtemSharp.Commands.Settings;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings;

[TestFixture]
public class TimeConfigCommandTests : SerializedCommandTestBase<TimeConfigCommand, TimeConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public TimeMode Mode { get; set; }
    }

    protected override TimeConfigCommand CreateSut(TestCaseData testCase)
    {
        return new TimeConfigCommand(new AtemState
        {
            Settings = new SettingsState
            {
                TimeMode = testCase.Command.Mode
            }
        });
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
    }
}
