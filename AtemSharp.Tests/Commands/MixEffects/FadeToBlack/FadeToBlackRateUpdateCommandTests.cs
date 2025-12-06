using AtemSharp.Commands.MixEffects.FadeToBlack;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.FadeToBlack;

public class FadeToBlackRateUpdateCommandTests : DeserializedCommandTestBase<FadeToBlackRateUpdateCommand,
    FadeToBlackRateUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; init; }
        public ushort Rate { get; init; }
    }

    protected override void CompareCommandProperties(FadeToBlackRateUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects = AtemStateUtil.CreateArray<MixEffect>(expectedData.Index + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var mixEffect = state.Video.MixEffects[expectedData.Index];
        Assert.That(mixEffect.Id, Is.EqualTo(expectedData.Index));
        Assert.That(mixEffect.FadeToBlack.Rate, Is.EqualTo(expectedData.Rate));
        Assert.That(mixEffect.FadeToBlack.InTransition, Is.False);
        Assert.That(mixEffect.FadeToBlack.IsFullyBlack, Is.False);
        Assert.That(mixEffect.FadeToBlack.RemainingFrames, Is.EqualTo(0));
    }
}
