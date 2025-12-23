using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class PreviewTransitionCommandTests : SerializedCommandTestBase<PreviewTransitionCommand,
    PreviewTransitionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public bool PreviewTransition { get; set; }
    }

    protected override PreviewTransitionCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new PreviewTransitionCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            TransitionPreview = testCase.Command.PreviewTransition
        });
    }
}
