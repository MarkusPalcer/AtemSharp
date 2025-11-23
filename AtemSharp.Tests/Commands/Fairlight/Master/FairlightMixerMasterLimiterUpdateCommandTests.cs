using FairlightMixerMasterLimiterUpdateCommand = AtemSharp.Commands.Fairlight.Master.FairlightMixerMasterLimiterUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

public class FairlightMixerMasterLimiterUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterLimiterUpdateCommand, FairlightMixerMasterLimiterUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool LimiterEnabled { get; set; }
        public double Threshold { get; set; }
        public double Attack { get; set; }
        public double Hold { get; set; }
        public double Release { get; set; }
    }


    protected override void CompareCommandProperties(FairlightMixerMasterLimiterUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.LimiterEnabled, Is.EqualTo(expectedData.LimiterEnabled));
        Assert.That(actualCommand.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
