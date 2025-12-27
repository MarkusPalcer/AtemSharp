using AtemSharp.Commands.DownstreamKey;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.DownstreamKey;

[TestFixture]
internal class DownstreamKeyPropertiesUpdateCommandTests : DeserializedCommandTestBase<DownstreamKeyPropertiesUpdateCommand,
    DownstreamKeyPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool Tie { get; set; }
        public int Rate { get; set; }
        public bool PreMultipliedKey { get; set; }
        public double Clip { get; set; }
        public double Gain { get; set; }
        public bool Invert { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    internal override void CompareCommandProperties(DownstreamKeyPropertiesUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.DownstreamKeyerId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Tie, Is.EqualTo(expectedData.Tie));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(actualCommand.PreMultiply, Is.EqualTo(expectedData.PreMultipliedKey));
        Assert.That(actualCommand.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(actualCommand.Invert, Is.EqualTo(expectedData.Invert));
        Assert.That(actualCommand.MaskEnabled, Is.EqualTo(expectedData.MaskEnabled));
        Assert.That(actualCommand.MaskTop, Is.EqualTo(expectedData.MaskTop).Within(0.001));
        Assert.That(actualCommand.MaskBottom, Is.EqualTo(expectedData.MaskBottom).Within(0.001));
        Assert.That(actualCommand.MaskLeft, Is.EqualTo(expectedData.MaskLeft).Within(0.001));
        Assert.That(actualCommand.MaskRight, Is.EqualTo(expectedData.MaskRight).Within(0.001));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.DownstreamKeyers.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.Video.DownstreamKeyers[expectedData.Index].Properties;
        Assert.That(target.Tie, Is.EqualTo(expectedData.Tie));
        Assert.That(target.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(target.PreMultiply, Is.EqualTo(expectedData.PreMultipliedKey));
        Assert.That(target.Clip, Is.EqualTo(expectedData.Clip).Within(0.1));
        Assert.That(target.Gain, Is.EqualTo(expectedData.Gain).Within(0.1));
        Assert.That(target.Invert, Is.EqualTo(expectedData.Invert));
        Assert.That(target.Mask.Enabled, Is.EqualTo(expectedData.MaskEnabled));
        Assert.That(target.Mask.Top, Is.EqualTo(expectedData.MaskTop).Within(0.001));
        Assert.That(target.Mask.Bottom, Is.EqualTo(expectedData.MaskBottom).Within(0.001));
        Assert.That(target.Mask.Left, Is.EqualTo(expectedData.MaskLeft).Within(0.001));
        Assert.That(target.Mask.Right, Is.EqualTo(expectedData.MaskRight).Within(0.001));
    }
}
