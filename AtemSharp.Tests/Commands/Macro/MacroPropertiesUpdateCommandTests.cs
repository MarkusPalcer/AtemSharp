using AtemSharp.Commands.Macro;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroPropertiesUpdateCommandTests : DeserializedCommandTestBase<MacroPropertiesUpdateCommand,
    MacroPropertiesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public bool IsUsed { get; set; }
        public bool HasUnsupportedOps { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    protected override void CompareCommandProperties(MacroPropertiesUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(actualCommand.HasUnsupportedOps, Is.EqualTo(expectedData.HasUnsupportedOps));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.Description, Is.EqualTo(expectedData.Description));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Macros.Macros = AtemStateUtil.CreateArray<AtemSharp.State.Macro.Macro>(expectedData.Index + 1);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Macros.Macros[expectedData.Index];
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(actualCommand.HasUnsupportedOps, Is.EqualTo(expectedData.HasUnsupportedOps));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.Description, Is.EqualTo(expectedData.Description));
    }
}
