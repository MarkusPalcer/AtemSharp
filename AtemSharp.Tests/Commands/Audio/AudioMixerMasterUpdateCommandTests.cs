using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerMasterUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerMasterUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
internal class AudioMixerMasterUpdateCommandTests : DeserializedCommandTestBase<AudioMixerMasterUpdateCommand,
    AudioMixerMasterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public double Gain { get; set; }
        public double Balance { get; set; }
        public bool FollowFadeToBlack { get; set; }
    }

    internal override void CompareCommandProperties(AudioMixerMasterUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.Balance, Is.EqualTo(expectedData.Balance).Within(0.01));
        Assert.That(actualCommand.FollowFadeToBlack, Is.EqualTo(expectedData.FollowFadeToBlack));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new ClassicAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetClassicAudio().Master.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(state.GetClassicAudio().Master.Balance, Is.EqualTo(expectedData.Balance).Within(0.01));
        Assert.That(state.GetClassicAudio().Master.FollowFadeToBlack, Is.EqualTo(expectedData.FollowFadeToBlack));
    }
}
