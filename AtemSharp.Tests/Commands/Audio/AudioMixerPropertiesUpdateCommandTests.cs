using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerPropertiesUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerPropertiesUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
internal class AudioMixerPropertiesUpdateCommandTests : DeserializedCommandTestBase<AudioMixerPropertiesUpdateCommand,
    AudioMixerPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool AudioFollowVideo { get; set; }
    }

    internal override void CompareCommandProperties(AudioMixerPropertiesUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.AudioFollowVideo, Is.EqualTo(expectedData.AudioFollowVideo));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new ClassicAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetClassicAudio().AudioFollowsVideo, Is.EqualTo(expectedData.AudioFollowVideo));
    }
}
