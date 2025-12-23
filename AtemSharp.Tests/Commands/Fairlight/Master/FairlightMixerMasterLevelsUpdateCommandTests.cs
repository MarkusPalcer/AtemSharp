using AtemSharp.State;
using FairlightMixerMasterLevelsUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Master.FairlightMixerMasterLevelsUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Master;

internal class FairlightMixerMasterLevelsUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerMasterLevelsUpdateCommand,
    FairlightMixerMasterLevelsUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public double InputLeftLevel { get; set; }
        public double InputRightLevel { get; set; }
        public double InputLeftPeak { get; set; }
        public double InputRightPeak { get; set; }
        public double CompressorGainReduction { get; set; }
        public double LimiterGainReduction { get; set; }
        public double OutputLeftLevel { get; set; }
        public double OutputRightLevel { get; set; }
        public double OutputLeftPeak { get; set; }
        public double OutputRightPeak { get; set; }
        public double LeftLevel { get; set; }
        public double RightLevel { get; set; }
        public double LeftPeak { get; set; }
        public double RightPeak { get; set; }
    }

    internal override void CompareCommandProperties(FairlightMixerMasterLevelsUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.InputLeftLevel, Is.EqualTo(expectedData.InputLeftLevel).Within(0.01));
        Assert.That(actualCommand.InputRightLevel, Is.EqualTo(expectedData.InputRightLevel).Within(0.01));
        Assert.That(actualCommand.InputLeftPeak, Is.EqualTo(expectedData.InputLeftPeak).Within(0.01));
        Assert.That(actualCommand.InputRightPeak, Is.EqualTo(expectedData.InputRightPeak).Within(0.01));
        Assert.That(actualCommand.CompressorGainReduction, Is.EqualTo(expectedData.CompressorGainReduction).Within(0.01));
        Assert.That(actualCommand.LimiterGainReduction, Is.EqualTo(expectedData.LimiterGainReduction).Within(0.01));
        Assert.That(actualCommand.OutputLeftLevel, Is.EqualTo(expectedData.OutputLeftLevel).Within(0.01));
        Assert.That(actualCommand.OutputRightLevel, Is.EqualTo(expectedData.OutputRightLevel).Within(0.01));
        Assert.That(actualCommand.OutputLeftPeak, Is.EqualTo(expectedData.OutputLeftPeak).Within(0.01));
        Assert.That(actualCommand.OutputRightPeak, Is.EqualTo(expectedData.OutputRightPeak).Within(0.01));
        Assert.That(actualCommand.LeftLevel, Is.EqualTo(expectedData.LeftLevel).Within(0.01));
        Assert.That(actualCommand.RightLevel, Is.EqualTo(expectedData.RightLevel).Within(0.01));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        // No state change
    }
}
