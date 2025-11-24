using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyPatternUpdateCommandTests :DeserializedCommandTestBase<MixEffectKeyPatternUpdateCommand, MixEffectKeyPatternUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public UpstreamKeyerPatternStyle Pattern { get; set; }
        public double Symmetry  { get; set; }
        public double Softness  { get; set; }
        public double XPosition  { get; set; }
        public double YPosition  { get; set; }
        public bool Inverse { get; set; }
        public double Size { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyPatternUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(testCase.Command.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(testCase.Command.KeyerIndex));
        Assert.That(actualCommand.Style, Is.EqualTo(testCase.Command.Pattern));
        Assert.That(actualCommand.Size, Is.EqualTo(testCase.Command.Size).Within(0.01));
        Assert.That(actualCommand.Symmetry, Is.EqualTo(testCase.Command.Symmetry).Within(0.01));
        Assert.That(actualCommand.Softness, Is.EqualTo(testCase.Command.Softness).Within(0.01));
        Assert.That(actualCommand.PositionX , Is.EqualTo(testCase.Command.XPosition).Within(0.0001));
        Assert.That(actualCommand.PositionY , Is.EqualTo(testCase.Command.YPosition).Within(0.0001));
        Assert.That(actualCommand.Invert,  Is.EqualTo(testCase.Command.Inverse));
    }
}
