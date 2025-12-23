using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerHeadphonesUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerHeadphonesUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
internal class AudioMixerHeadphonesUpdateCommandTests : DeserializedCommandTestBase<AudioMixerHeadphonesUpdateCommand,
    AudioMixerHeadphonesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public double Gain { get; set; }
        public double ProgramOutGain { get; set; }
        public double TalkbackGain { get; set; }
        public double SidetoneGain { get; set; }
    }

    internal override void CompareCommandProperties(AudioMixerHeadphonesUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.ProgramOutGain, Is.EqualTo(expectedData.ProgramOutGain).Within(0.01));
        Assert.That(actualCommand.TalkbackGain, Is.EqualTo(expectedData.TalkbackGain).Within(0.01));
        Assert.That(actualCommand.SidetoneGain, Is.EqualTo(expectedData.SidetoneGain).Within(0.01));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new ClassicAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetClassicAudio().Headphones.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(state.GetClassicAudio().Headphones.ProgramOutGain, Is.EqualTo(expectedData.ProgramOutGain).Within(0.01));
        Assert.That(state.GetClassicAudio().Headphones.TalkbackGain, Is.EqualTo(expectedData.TalkbackGain).Within(0.01));
        Assert.That(state.GetClassicAudio().Headphones.SidetoneGain, Is.EqualTo(expectedData.SidetoneGain).Within(0.01));
    }
}
