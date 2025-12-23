using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerPropertiesCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerPropertiesCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerPropertiesCommandTests : SerializedCommandTestBase<AudioMixerPropertiesCommand,
    AudioMixerPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool AudioFollowVideo { get; set; }
    }

    protected override AudioMixerPropertiesCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new AudioMixerPropertiesCommand(new ClassicAudioState
        {
            AudioFollowsVideo = testCase.Command.AudioFollowVideo
        });
    }

    [Test]
    public void AudioFollowVideo_WhenSet_ShouldSetFlag()
    {
        // Arrange
        var command = new AudioMixerPropertiesCommand(new());

        // Act
        command.AudioFollowVideo = true;

        // Assert
        Assert.That(command.Flag & (1 << 0), Is.Not.EqualTo(0), "Flag bit 0 should be set");
    }
}
