using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class AudioMixerConfigCommandTests : DeserializedCommandTestBase<AudioMixerConfigCommand, AudioMixerConfigCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Inputs { get; set; }
        public byte Monitors { get; set; }
        public byte Headphones { get; set; }
    }

    protected override void CompareCommandProperties(AudioMixerConfigCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

        // Compare Headphones
        if (actualCommand.Headphones != expectedData.Headphones)
        {
            failures.Add($"Headphones: expected {expectedData.Headphones}, actual {actualCommand.Headphones}");
        }

        if (failures.Count > 0)
        {
            Assert.Fail($"Command property comparison failed:\n{string.Join("\n", failures)}");
        }
    }

    [Test]
    public void ApplyToState_WithValidData_ShouldUpdateInfoAndAudioState()
    {
        // Arrange
        var state = new AtemState();
        var command = new AudioMixerConfigCommand
        {
            Inputs = 20,
            Monitors = 2,
            Headphones = 1
        };

        // Act
        command.ApplyToState(state);

        // Assert
        var mixer =  state.Info.Mixer as AudioMixerInfo;
        Assert.That(mixer, Is.Not.Null);
        Assert.That(mixer.Inputs, Is.EqualTo(20));
        Assert.That(mixer.Monitors, Is.EqualTo(2));
        Assert.That(mixer.Headphones, Is.EqualTo(1));

        var classicAudioState = state.Audio.As<ClassicAudioState>();
        Assert.That(classicAudioState.Channels, Is.Not.Null);
        Assert.That(classicAudioState.Channels, Is.Empty);
    }
}
