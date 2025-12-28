using AtemSharp.Commands.Macro;
using NSubstitute;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroRecordCommandTests : SerializedCommandTestBase<MacroRecordCommand, MacroRecordCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    protected override MacroRecordCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        var macro = new AtemSharp.State.Macro.Macro(Substitute.For<IAtemSwitcher>())
        {
            Id = testCase.Command.Index,
        };

        macro.UpdateName(testCase.Command.Name);
        macro.UpdateDescription(testCase.Command.Description);

        return new MacroRecordCommand(macro);
    }
}
