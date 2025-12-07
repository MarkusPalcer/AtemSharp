using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

public class MixEffectKeyPatternUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyPatternUpdateCommand,
    MixEffectKeyPatternUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public UpstreamKeyerPatternStyle Pattern { get; set; }
        public double Symmetry { get; set; }
        public double Softness { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public bool Inverse { get; set; }
        public double Size { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyPatternUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.Style, Is.EqualTo(expectedData.Pattern));
        Assert.That(actualCommand.Size, Is.EqualTo(expectedData.Size).Within(0.01));
        Assert.That(actualCommand.Symmetry, Is.EqualTo(expectedData.Symmetry).Within(0.01));
        Assert.That(actualCommand.Softness, Is.EqualTo(expectedData.Softness).Within(0.01));
        Assert.That(actualCommand.PositionX, Is.EqualTo(expectedData.XPosition).Within(0.0001));
        Assert.That(actualCommand.PositionY, Is.EqualTo(expectedData.YPosition).Within(0.0001));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Inverse));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects = AtemStateUtil.CreateArray<MixEffect>(expectedData.MixEffectIndex + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.MixEffectIndex].UpstreamKeyers[expectedData.KeyerIndex].Pattern;
        Assert.That(actualCommand.Style, Is.EqualTo(expectedData.Pattern));
        Assert.That(actualCommand.Size, Is.EqualTo(expectedData.Size).Within(0.01));
        Assert.That(actualCommand.Symmetry, Is.EqualTo(expectedData.Symmetry).Within(0.01));
        Assert.That(actualCommand.Softness, Is.EqualTo(expectedData.Softness).Within(0.01));
        Assert.That(actualCommand.Location.X, Is.EqualTo(expectedData.XPosition).Within(0.0001));
        Assert.That(actualCommand.Location.Y, Is.EqualTo(expectedData.YPosition).Within(0.0001));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Inverse));
    }
}
