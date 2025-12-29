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
        return testCase.Command.Action switch
        {
            MacroAction.Run => MacroActionCommand.Run(macro),
            MacroAction.Stop => MacroActionCommand.Stop(),
            MacroAction.StopRecord => MacroActionCommand.StopRecord(),
            MacroAction.InsertUserWait => MacroActionCommand.InsertUserWait(),
            MacroAction.Continue => MacroActionCommand.Continue(),
            MacroAction.Delete => MacroActionCommand.Delete(macro),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
