using AudioMixerMasterUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerMasterUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerMasterUpdateCommandTests : DeserializedCommandTestBase<AudioMixerMasterUpdateCommand,
    AudioMixerMasterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public double Gain { get; set; }
        public double Balance { get; set; }
        public bool FollowFadeToBlack { get; set; }
    }

    protected override void CompareCommandProperties(AudioMixerMasterUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.Balance, Is.EqualTo(expectedData.Balance).Within(0.01));
        Assert.That(actualCommand.FollowFadeToBlack, Is.EqualTo(expectedData.FollowFadeToBlack));
    }
}
