using AtemSharp.Commands.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight;

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
        Assert.That(actualCommand.Parameters.LimiterEnabled, Is.EqualTo(expectedData.LimiterEnabled));
        Assert.That(actualCommand.Parameters.Threshold, Is.EqualTo(expectedData.Threshold).Within(0.01));
        Assert.That(actualCommand.Parameters.Attack, Is.EqualTo(expectedData.Attack).Within(0.01));
        Assert.That(actualCommand.Parameters.Hold, Is.EqualTo(expectedData.Hold).Within(0.01));
        Assert.That(actualCommand.Parameters.Release, Is.EqualTo(expectedData.Release).Within(0.01));
    }
}
