using AtemSharp.State;
using FairlightMixerSourceLevelsUpdateCommand = AtemSharp.Commands.Audio.Fairlight.Source.FairlightMixerSourceLevelsUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.Source;

internal class FairlightMixerSourceLevelsUpdateCommandTests : DeserializedCommandTestBase<FairlightMixerSourceLevelsUpdateCommand,
    FairlightMixerSourceLevelsUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public long SourceId { get; set; }

        public double InputLeftLevel { get; set; }
        public double InputRightLevel { get; set; }
        public double InputLeftPeak { get; set; }
        public double InputRightPeak { get; set; }
        public double CompressorGainReduction { get; set; }
        public double LimiterGainReduction { get; set; }
        public double ExpanderGainReduction { get; set; }
        public double OutputLeftLevel { get; set; }
        public double OutputRightLevel { get; set; }
        public double OutputLeftPeak { get; set; }
        public double OutputRightPeak { get; set; }

        public double LeftLevel { get; set; }
        public double RightLevel { get; set; }
        public double LeftPeak { get; set; }
        public double RightPeak { get; set; }
    }

    internal override void CompareCommandProperties(FairlightMixerSourceLevelsUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.InputId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceId, Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.InputLeftLevel, Is.EqualTo(expectedData.InputLeftLevel).Within(0.01));
        Assert.That(actualCommand.InputRightLevel, Is.EqualTo(expectedData.InputRightLevel).Within(0.01));
        Assert.That(actualCommand.InputLeftPeak, Is.EqualTo(expectedData.InputLeftPeak).Within(0.01));
        Assert.That(actualCommand.InputRightPeak, Is.EqualTo(expectedData.InputRightPeak).Within(0.01));
        Assert.That(actualCommand.CompressorGainReduction, Is.EqualTo(expectedData.CompressorGainReduction).Within(0.01));
        Assert.That(actualCommand.LimiterGainReduction, Is.EqualTo(expectedData.LimiterGainReduction).Within(0.01));
        Assert.That(actualCommand.ExpanderGainReduction, Is.EqualTo(expectedData.ExpanderGainReduction).Within(0.01));
        Assert.That(actualCommand.OutputLeftLevel, Is.EqualTo(expectedData.OutputLeftLevel).Within(0.01));
        Assert.That(actualCommand.OutputRightLevel, Is.EqualTo(expectedData.OutputRightLevel).Within(0.01));
        Assert.That(actualCommand.OutputLeftPeak, Is.EqualTo(expectedData.OutputLeftPeak).Within(0.01));
        Assert.That(actualCommand.OutputRightPeak, Is.EqualTo(expectedData.OutputRightPeak).Within(0.01));
        Assert.That(actualCommand.LeftLevel, Is.EqualTo(expectedData.LeftLevel).Within(0.01));
        Assert.That(actualCommand.RightLevel, Is.EqualTo(expectedData.RightLevel).Within(0.01));
        Assert.That(actualCommand.LeftPeak, Is.EqualTo(expectedData.LeftPeak).Within(0.01));
        Assert.That(actualCommand.RightPeak, Is.EqualTo(expectedData.RightPeak).Within(0.01));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        // No change to state
    }
}
