using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
internal class TransitionMixUpdateCommandTests : DeserializedCommandTestBase<TransitionMixUpdateCommand,
    TransitionMixUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public int Rate { get; set; }
    }

    internal override void CompareCommandProperties(TransitionMixUpdateCommand actualCommand, CommandData expectedData)
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
        Assert.That(state.Video.MixEffects[expectedData.Index].TransitionSettings.Mix.Rate, Is.EqualTo(expectedData.Rate));
    }
}
