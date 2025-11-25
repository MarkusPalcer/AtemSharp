using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class FairlightAudioMixerConfigCommandTests : DeserializedCommandTestBase<FairlightAudioMixerConfigCommand,
    FairlightAudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
    }

    protected override void CompareCommandProperties(FairlightAudioMixerConfigCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        var failures = new List<string>();

        // Compare Inputs
        if (actualCommand.Inputs != expectedData.Inputs)
        {
            failures.Add($"Inputs: expected {expectedData.Inputs}, actual {actualCommand.Inputs}");
        }

        // Compare Monitors
        if (actualCommand.Monitors != expectedData.Monitors)
        {
            failures.Add($"Monitors: expected {expectedData.Monitors}, actual {actualCommand.Monitors}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateInfoAndFairlightState()
    {
        // Arrange
        var state = new AtemState();
        var command = new FairlightAudioMixerConfigCommand
        {
            Inputs = 24,
            Monitors = 4
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var mixer = state.Info.Mixer.As<FairlightAudioMixerInfo>();
        Assert.That(mixer.Inputs, Is.EqualTo(24));
        Assert.That(mixer.Monitors, Is.EqualTo(4));

        Assert.That(state.Audio.As<FairlightAudioState>().Inputs, Is.Not.Null);
    }

    [Test]
    public void ApplyToState_MultipleCallsShouldOverwritePreviousState()
    {
        // Arrange
        var state = new AtemState();
        var command1 = new FairlightAudioMixerConfigCommand { Inputs = 12, Monitors = 2 };
        var command2 = new FairlightAudioMixerConfigCommand { Inputs = 24, Monitors = 4 };

        // Act
        command1.ApplyToState(state);
        command2.ApplyToState(state);

        // Assert - Should have the latest values
        var mixer = state.Info.Mixer.As<FairlightAudioMixerInfo>();
        Assert.That(mixer.Inputs, Is.EqualTo(24));
        Assert.That(mixer.Monitors, Is.EqualTo(4));
        Assert.That(state.Audio.As<FairlightAudioState>(), Is.Not.Null);
    }
}
