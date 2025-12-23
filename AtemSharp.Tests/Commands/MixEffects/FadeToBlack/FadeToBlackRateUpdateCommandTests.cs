using AtemSharp.Commands.MixEffects.FadeToBlack;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.FadeToBlack;

internal class FadeToBlackRateUpdateCommandTests : DeserializedCommandTestBase<FadeToBlackRateUpdateCommand,
    FadeToBlackRateUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; init; }
        public ushort Rate { get; init; }
    }

    internal override void CompareCommandProperties(FadeToBlackRateUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Rate, Is.EqualTo(expectedData.Rate));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.Index);
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
