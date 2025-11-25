using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

public class AutoTransitionCommandTests : SerializedCommandTestBase<AutoTransitionCommand, AutoTransitionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
    }

    protected override AutoTransitionCommand CreateSut(TestCaseData testCase)
    {
        return new AutoTransitionCommand(new MixEffect { Id = testCase.Command.Index });
    }
}
