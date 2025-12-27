using AtemSharp.Commands.Macro;
using NSubstitute;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroActionCommandTests : SerializedCommandTestBase<MacroActionCommand, MacroActionCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }

        public MacroAction Action { get; set; }
    }

    protected override MacroActionCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        var macro = new AtemSharp.State.Macro.Macro(Substitute.For<IAtemSwitcher>()) { Id = testCase.Command.Index };
        return new MacroActionCommand(macro, testCase.Command.Action);
    }
}
