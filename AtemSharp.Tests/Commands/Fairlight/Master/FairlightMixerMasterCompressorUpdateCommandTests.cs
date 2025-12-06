using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterCompressorUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterCompressorUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterCompressorUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterCompressorUpdateCommand,
    FairlightMixerMasterCompressorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool CompressorEnabled { get; set; }
        public double Threshold { get; set; }
        public double Ratio { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }

    protected override void CompareCommandProperties(FairlightMixerMasterCompressorUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.CompressorEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Ratio, Is.EqualTo(expectedData.Ratio).Within(0.01));
        Assert.That(actualCommand.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.GetFairlight().Master.Dynamics.Compressor;
        Assert.That(target.Enabled, Is.EqualTo(expectedData.CompressorEnabled));
        Assert.That(target.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(target.Ratio, Is.EqualTo(expectedData.Ratio).Within(0.01));
        Assert.That(target.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(target.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(target.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
