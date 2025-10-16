using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
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
        Assert.That(state.Info.AudioMixer, Is.Not.Null);
        Assert.That(state.Info.AudioMixer.Inputs, Is.EqualTo(20));
        Assert.That(state.Info.AudioMixer.Monitors, Is.EqualTo(2));
        Assert.That(state.Info.AudioMixer.Headphones, Is.EqualTo(1));

        Assert.That(state.Audio, Is.Not.Null);
        Assert.That(state.Audio.As<ClassicAudioState>().Channels, Is.Not.Null);
        Assert.That(state.Audio.As<ClassicAudioState>().Channels, Is.Empty);
    }
}
