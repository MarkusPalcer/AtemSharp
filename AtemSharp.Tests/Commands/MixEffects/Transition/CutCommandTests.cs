using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State.Video.MixEffect;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

public class CutCommandTests : SerializedCommandTestBase<CutCommand, CutCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public byte Index { get; set; }
    }

    protected override CutCommand CreateSut(TestCaseData testCase)
    {
        return new CutCommand(new MixEffect { Id = testCase.Command.Index });
    }
}
