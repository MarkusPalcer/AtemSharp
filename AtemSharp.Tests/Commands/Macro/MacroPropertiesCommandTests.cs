using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroPropertiesCommandTests : SerializedCommandTestBase<MacroPropertiesCommand, MacroPropertiesCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    protected override MacroPropertiesCommand CreateSut(TestCaseData testCase)
    {
        var macro = new AtemSharp.State.Macro
        {
            Id = testCase.Command.Index,
            Name = testCase.Command.Name,
            Description = testCase.Command.Description
        };
        return new MacroPropertiesCommand(macro);
    }
}
