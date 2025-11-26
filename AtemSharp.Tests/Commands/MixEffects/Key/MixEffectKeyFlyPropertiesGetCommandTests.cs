using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyFlyPropertiesGetCommandTests : DeserializedCommandTestBase<MixEffectKeyFlyPropertiesGetCommand,
    MixEffectKeyFlyPropertiesGetCommandTests.CommandData>
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

    protected override void CompareCommandProperties(MixEffectKeyFlyPropertiesGetCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectIndex, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerIndex, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.IsASet, Is.EqualTo(expectedData.IsASet));
        Assert.That(actualCommand.IsBSet, Is.EqualTo(expectedData.IsBSet));
        Assert.That(actualCommand.IsAtKeyFrame, Is.EqualTo(expectedData.RunningToKeyFrame));
        Assert.That(actualCommand.RunToInfiniteIndex, Is.EqualTo(expectedData.RunningToInfinite));
    }
}
