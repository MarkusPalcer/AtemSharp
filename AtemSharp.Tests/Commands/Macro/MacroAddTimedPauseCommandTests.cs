using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroAddTimedPauseCommandTests : SerializedCommandTestBase<MacroAddTimedPauseCommand, MacroAddTimedPauseCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
public ushort Frames { get; set; }
    }

    protected override MacroAddTimedPauseCommand CreateSut(TestCaseData testCase)
    {
        return new MacroAddTimedPauseCommand
        {
            Frames = testCase.Command.Frames
        };
    }
}
