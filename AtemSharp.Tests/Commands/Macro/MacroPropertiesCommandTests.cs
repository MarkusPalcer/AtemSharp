using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroPropertiesCommandTests : SerializedCommandTestBase<MacroPropertiesCommand, MacroPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    protected override MacroPropertiesCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MacroPropertiesCommand(new AtemSharp.State.Macro.Macro
        {
            Id = testCase.Command.Index,
            Name = testCase.Command.Name,
            Description = testCase.Command.Description
        });
    }
}
