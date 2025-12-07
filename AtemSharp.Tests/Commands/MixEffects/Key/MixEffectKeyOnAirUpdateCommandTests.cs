using AtemSharp.Commands.MixEffects.Key;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Key;

[TestFixture]
public class MixEffectKeyOnAirUpdateCommandTests : DeserializedCommandTestBase<MixEffectKeyOnAirUpdateCommand,
    MixEffectKeyOnAirUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte MixEffectIndex { get; set; }
        public byte KeyerIndex { get; set; }
        public bool OnAir { get; set; }
    }

    protected override void CompareCommandProperties(MixEffectKeyOnAirUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.KeyerId, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.OnAir, Is.EqualTo(expectedData.OnAir));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects = AtemStateUtil.CreateArray<MixEffect>(expectedData.MixEffectIndex + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.MixEffectIndex].UpstreamKeyers[expectedData.KeyerIndex];
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.MixEffectIndex));
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.KeyerIndex));
        Assert.That(actualCommand.OnAir, Is.EqualTo(expectedData.OnAir));
    }
}
