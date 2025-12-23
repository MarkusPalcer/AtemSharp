using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroAddTimedPauseCommandTests : SerializedCommandTestBase<MacroAddTimedPauseCommand,
    MacroAddTimedPauseCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Frames { get; set; }
    }

    protected override MacroAddTimedPauseCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MacroAddTimedPauseCommand
        {
            Frames = testCase.Command.Frames
        };
    }
}
