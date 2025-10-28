using AtemSharp.Commands.Settings;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Settings;

[TestFixture]
public class TimeConfigUpdateCommandTests : DeserializedCommandTestBase<TimeConfigUpdateCommand, TimeConfigUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public TimeMode Mode { get; set; }
    }

    protected override void CompareCommandProperties(TimeConfigUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.Mode, Is.EqualTo(expectedData.Mode),
                   $"Mode should match expected value for test case {testCase.Name}");
    }

    [Test]
    public void ApplyToState_WithExistingSettings_UpdatesTimeMode()
    {
        // Arrange
        var state = new AtemState
        {
            Settings = new SettingsState
            {
                TimeMode = TimeMode.FreeRun
            }
        };

        var command = new TimeConfigUpdateCommand
        {
            Mode = TimeMode.TimeOfDay
        };

        // Act
        command.ApplyToState(state);

        // Assert
        Assert.That(state.Settings.TimeMode, Is.EqualTo(TimeMode.TimeOfDay));
    }
}
