using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
internal class PreviewTransitionUpdateCommandTests : DeserializedCommandTestBase<PreviewTransitionUpdateCommand,
    PreviewTransitionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool PreviewTransition { get; set; }
    }

    internal override void CompareCommandProperties(PreviewTransitionUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Preview, Is.EqualTo(expectedData.PreviewTransition));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects.GetOrCreate(expectedData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Video.MixEffects[expectedData.Index].TransitionPreview, Is.EqualTo(expectedData.PreviewTransition));
    }
}
