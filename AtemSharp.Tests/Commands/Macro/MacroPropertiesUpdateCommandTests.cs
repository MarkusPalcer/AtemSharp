using AtemSharp.Commands.Macro;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.Macro;

internal class MacroPropertiesUpdateCommandTests : DeserializedCommandTestBase<MacroPropertiesUpdateCommand,
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

    internal override void CompareCommandProperties(MacroPropertiesUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.IsUsed, Is.EqualTo(expectedData.IsUsed));
        Assert.That(actualCommand.HasUnsupportedOps, Is.EqualTo(expectedData.HasUnsupportedOps));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.Description, Is.EqualTo(expectedData.Description));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
    }

    protected override void PrepareState(IStateHolder stateHolder, CommandData testData)
    {
        stateHolder.Macros.GetOrCreate(testData.Index);
    }

    protected override void CompareStateProperties(IStateHolder stateHolder, CommandData testData)
    {
        var macro = stateHolder.Macros[testData.Index];
        Assert.That(macro.IsUsed, Is.EqualTo(testData.IsUsed));
        Assert.That(macro.HasUnsupportedOps, Is.EqualTo(testData.HasUnsupportedOps));
        Assert.That(macro.Name, Is.EqualTo(testData.Name));
        Assert.That(macro.Description, Is.EqualTo(testData.Description));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
    }

    [Test]
    public void EmptyNameAndDescription()
    {
        var data = (byte[])
        [
            0, 0, // ID = 0
            0, // IsUsed = false
            0, // HasUnsupportedOos = false
            0, 0, // Name length = 0
            0, 0, // Description length = 0
        ];

        var command = MacroPropertiesUpdateCommand.Deserialize(data, ProtocolVersion.Unknown).As<MacroPropertiesUpdateCommand>();

        Assert.That(command.Id, Is.EqualTo(0));
        Assert.That(command.IsUsed, Is.EqualTo(false));
        Assert.That(command.HasUnsupportedOps, Is.EqualTo(false));
        Assert.That(command.Name, Is.EqualTo(string.Empty));
        Assert.That(command.Description, Is.EqualTo(string.Empty));


        var state = new TestStateHolder();
        var macro = state.Macros.GetOrCreate(0);
        macro.Id = 0;
        macro.UpdateIsUsed(true);
        macro.UpdateHasUnsupportedOps(true);
        macro.UpdateName("WrongName");
        macro.UpdateDescription("WrongDescription");

        command.Apply(state);
        Assert.That(state.Macros.Count, Is.EqualTo(1));
        Assert.That(state.Macros[0].IsUsed, Is.False);
        Assert.That(state.Macros[0].HasUnsupportedOps, Is.False);
        Assert.That(state.Macros[0].Name, Is.EqualTo(string.Empty));
        Assert.That(state.Macros[0].Description, Is.EqualTo(string.Empty));
    }

    [Test]
    public void MissingMacroEntry()
    {
        var sut = new MacroPropertiesUpdateCommand
        {
            Id = 1,
            IsUsed = false,
            HasUnsupportedOps = false,
            Name = "Name",
            Description = "Description"
        };

        var state = new TestStateHolder();
        Assert.Throws<KeyNotFoundException>(() => sut.Apply(state));
    }
}
