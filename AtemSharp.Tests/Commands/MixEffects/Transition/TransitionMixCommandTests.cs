using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

[TestFixture]
public class TransitionMixCommandTests : SerializedCommandTestBase<TransitionMixCommand,
    TransitionMixCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
        public byte Rate { get; set; }
    }

    protected override TransitionMixCommand CreateSut(TestCaseData testCase)
    {
        return new TransitionMixCommand(new MixEffect
        {
            Id = testCase.Command.Index,
            TransitionSettings =
            {
                Mix =
                {
                    Rate = testCase.Command.Rate
                }
            }
        });
    }
}
