using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
internal class MixEffectKeyFlyPropertiesUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyFlyPropertiesUpdateCommand,
    MixEffectKeyFlyPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool IsASet { get; set; }
        public bool IsBSet { get; set; }
        public IsAtKeyFrame RunningToKeyFrame { get; set; }
        public byte RunningToInfinite { get; set; }
    }

    internal override void CompareCommandProperties(MixEffectKeyFlyPropertiesUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MixEffectIndex, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerIndex, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.IsASet, Is.EqualTo(expectedData.IsASet));
        Assert.That(actualCommand.IsBSet, Is.EqualTo(expectedData.IsBSet));
        Assert.That(actualCommand.IsAtKeyFrame, Is.EqualTo(expectedData.RunningToKeyFrame));
        Assert.That(actualCommand.RunToInfiniteIndex, Is.EqualTo(expectedData.RunningToInfinite));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.MixEffectIndex);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.MixEffectIndex].UpstreamKeyers[expectedData.KeyerIndex].FlyProperties;
        Assert.That(actualCommand.IsASet, Is.EqualTo(expectedData.IsASet));
        Assert.That(actualCommand.IsBSet, Is.EqualTo(expectedData.IsBSet));
        Assert.That(actualCommand.IsAtKeyFrame, Is.EqualTo(expectedData.RunningToKeyFrame));
        Assert.That(actualCommand.RunToInfiniteIndex, Is.EqualTo(expectedData.RunningToInfinite));
    }
}
