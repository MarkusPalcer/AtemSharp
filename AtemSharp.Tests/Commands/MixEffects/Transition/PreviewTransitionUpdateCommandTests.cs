using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class PreviewTransitionUpdateCommandTests : DeserializedCommandTestBase<PreviewTransitionUpdateCommand,
    PreviewTransitionUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public int Index { get; set; }
        public bool PreviewTransition { get; set; }
    }

    protected override void CompareCommandProperties(PreviewTransitionUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.MixEffectId, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.Preview, Is.EqualTo(expectedData.PreviewTransition));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Video.MixEffects = AtemStateUtil.CreateArray<MixEffect>(expectedData.Index + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Video.MixEffects[expectedData.Index].TransitionPreview, Is.EqualTo(expectedData.PreviewTransition));
    }
}
