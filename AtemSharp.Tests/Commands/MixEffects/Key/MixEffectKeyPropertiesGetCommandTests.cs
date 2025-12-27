using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
internal class MixEffectKeyPropertiesGetCommandTests : DeserializedCommandTestBase<MixEffectKeyPropertiesGetCommand,
    MixEffectKeyPropertiesGetCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public MixEffectKeyType KeyType { get; set; }
        public bool CanFlyKey { get; set; }
        public bool FlyEnabled { get; set; }
        public int FillSource { get; set; }
        public int CutSource { get; set; }
        public bool MaskEnabled { get; set; }
        public double MaskTop { get; set; }
        public double MaskBottom { get; set; }
        public double MaskLeft { get; set; }
        public double MaskRight { get; set; }
    }

    internal override void CompareCommandProperties(MixEffectKeyPropertiesGetCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MixEffectIndex, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerIndex, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.KeyType, Is.EqualTo(expectedData.KeyType));
        Assert.That(actualCommand.CanFlyKey, Is.EqualTo(expectedData.CanFlyKey));
        Assert.That(actualCommand.FlyEnabled, Is.EqualTo(expectedData.FlyEnabled));
        Assert.That(actualCommand.FillSource, Is.EqualTo(expectedData.FillSource));
        Assert.That(actualCommand.CutSource, Is.EqualTo(expectedData.CutSource));
        Assert.That(actualCommand.MaskEnabled, Is.EqualTo(expectedData.MaskEnabled));
        Assert.That(actualCommand.MaskTop, Is.EqualTo(expectedData.MaskTop).Within(0.001));
        Assert.That(actualCommand.MaskBottom, Is.EqualTo(expectedData.MaskBottom).Within(0.001));
        Assert.That(actualCommand.MaskLeft, Is.EqualTo(expectedData.MaskLeft).Within(0.001));
        Assert.That(actualCommand.MaskRight, Is.EqualTo(expectedData.MaskRight).Within(0.001));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.MixEffectIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.MixEffectIndex].UpstreamKeyers[expectedData.KeyerIndex];
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.KeyType, Is.EqualTo(expectedData.KeyType));
        Assert.That(actualCommand.CanFlyKey, Is.EqualTo(expectedData.CanFlyKey));
        Assert.That(actualCommand.FlyEnabled, Is.EqualTo(expectedData.FlyEnabled));
        Assert.That(actualCommand.FillSource, Is.EqualTo(expectedData.FillSource));
        Assert.That(actualCommand.CutSource, Is.EqualTo(expectedData.CutSource));
        Assert.That(actualCommand.Mask.Enabled, Is.EqualTo(expectedData.MaskEnabled));
        Assert.That(actualCommand.Mask.Top, Is.EqualTo(expectedData.MaskTop).Within(0.001));
        Assert.That(actualCommand.Mask.Bottom, Is.EqualTo(expectedData.MaskBottom).Within(0.001));
        Assert.That(actualCommand.Mask.Left, Is.EqualTo(expectedData.MaskLeft).Within(0.001));
        Assert.That(actualCommand.Mask.Right, Is.EqualTo(expectedData.MaskRight).Within(0.001));
    }
}
