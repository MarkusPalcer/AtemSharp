using AtemSharp.Commands.MixEffects.FadeToBlack;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.FadeToBlack;

internal class FadeToBlackStateCommandTests : DeserializedCommandTestBase<FadeToBlackStateCommand, FadeToBlackStateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool IsFullyBlack { get; set; }
        public bool InTransition { get; set; }
        public byte RemainingFrames { get; set; }
    }

    internal override void CompareCommandProperties(FadeToBlackStateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsFullyBlack, Is.EqualTo(expectedData.IsFullyBlack));
        Assert.That(actualCommand.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Video.MixEffects[expectedData.Index];
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.FadeToBlack.IsFullyBlack, Is.EqualTo(expectedData.IsFullyBlack));
        Assert.That(actualCommand.FadeToBlack.InTransition, Is.EqualTo(expectedData.InTransition));
        Assert.That(actualCommand.FadeToBlack.RemainingFrames, Is.EqualTo(expectedData.RemainingFrames));
    }
}
