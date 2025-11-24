using AtemSharp.Commands.MixEffects.Transition;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.MixEffects.Transition;

public class CutCommandTests  : SerializedCommandTestBase<CutCommand, CutCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
public byte Index  { get; set; }
    }

    protected override CutCommand CreateSut(TestCaseData testCase)
    {
        return new CutCommand(new MixEffect {Index = testCase.Command.Index});
    }
}
