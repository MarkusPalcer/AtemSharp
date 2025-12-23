using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using FairlightMixerMasterLimiterUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterLimiterUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

internal class FairlightMixerMasterLimiterUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterLimiterUpdateCommand,
    FairlightMixerMasterLimiterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool LimiterEnabled { get; set; }
        public double Threshold { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }


    internal override void CompareCommandProperties(FairlightMixerMasterLimiterUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.LimiterEnabled, Is.EqualTo(expectedData.LimiterEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
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
        var target = state.GetFairlight().Master.Dynamics.Limiter;
        Assert.That(target.Enabled, Is.EqualTo(expectedData.LimiterEnabled));
        Assert.That(target.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(target.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(target.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(target.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
