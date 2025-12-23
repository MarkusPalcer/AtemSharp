using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroRunStatusCommandTests : SerializedCommandTestBase<MacroRunStatusCommand, MacroRunStatusCommandTests.CommandData> {
    public class CommandData : CommandDataBase
    {
        public bool Loop { get; set; }
    }

    protected override MacroRunStatusCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MacroRunStatusCommand
        {
            Loop = testCase.Command.Loop
        };
    }
}
