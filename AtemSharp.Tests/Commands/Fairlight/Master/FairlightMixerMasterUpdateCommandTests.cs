using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterUpdateCommand,
    FairlightMixerMasterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool EqualizerEnabled { get; set; }
        public double EqualizerGain { get; set; }
        public byte EqualizerBands { get; set; }
        public double MakeUpGain { get; set; }
        public bool FollowFadeToBlack { get; set; }
        public double Gain { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerMasterUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.EqualizerEnabled, Is.EqualTo(expectedData.EqualizerEnabled));
        Assert.That(actualCommand.EqualizerGain, Is.EqualTo(expectedData.EqualizerGain).Within(0.01));
        Assert.That(actualCommand.BandCount, Is.EqualTo(expectedData.EqualizerBands));
        Assert.That(actualCommand.MakeUpGain, Is.EqualTo(expectedData.MakeUpGain).Within(0.01));
        Assert.That(actualCommand.FaderGain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.FollowFadeToBlack, Is.EqualTo(expectedData.FollowFadeToBlack));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetFairlight().Master.Equalizer.Enabled, Is.EqualTo(expectedData.EqualizerEnabled));
        Assert.That(state.GetFairlight().Master.Equalizer.Gain, Is.EqualTo(expectedData.EqualizerGain).Within(0.01));
        Assert.That(state.GetFairlight().Master.Equalizer.Bands, Has.Length.EqualTo(expectedData.EqualizerBands));
        Assert.That(state.GetFairlight().Master.Dynamics.MakeUpGain, Is.EqualTo(expectedData.MakeUpGain).Within(0.01));
        Assert.That(state.GetFairlight().Master.FaderGain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(state.GetFairlight().Master.FollowFadeToBlack, Is.EqualTo(expectedData.FollowFadeToBlack));
    }
}
