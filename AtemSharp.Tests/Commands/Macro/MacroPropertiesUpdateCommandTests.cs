using AtemSharp.Commands.Macro;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

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

    [Test]
    public void EmptyNameAndDescription()
    {
        var data = (byte[])[
            0, 0, // ID = 0
            0,    // IsUsed = false
            0,    // HasUnsupportedOos = false
            0, 0, // Name length = 0
            0, 0, // Description length = 0
        ];

        var command = MacroPropertiesUpdateCommand.Deserialize(data, ProtocolVersion.Unknown).As<MacroPropertiesUpdateCommand>();

        Assert.That(command.Id, Is.EqualTo(0));
        Assert.That(command.IsUsed, Is.EqualTo(false));
        Assert.That(command.HasUnsupportedOps, Is.EqualTo(false));
        Assert.That(command.Name, Is.EqualTo(string.Empty));
        Assert.That(command.Description, Is.EqualTo(string.Empty));

        var state = new AtemState
        {
            Macros =
            {
                Macros = [new AtemSharp.State.Macro.Macro
                {
                    Id = 0,
                    IsUsed = true,
                    HasUnsupportedOps = true,
                    Name = "WrongName",
                    Description = "WrongDescription"
                }]
            }
        };

        command.ApplyToState(state);
        Assert.That(state.Macros.Macros.Count, Is.EqualTo(1));
        Assert.That(state.Macros.Macros[0].IsUsed, Is.False);
        Assert.That(state.Macros.Macros[0].HasUnsupportedOps, Is.False);
        Assert.That(state.Macros.Macros[0].Name, Is.EqualTo(string.Empty));
        Assert.That(state.Macros.Macros[0].Description, Is.EqualTo(string.Empty));
    }

    [Test]
    public void MissingMacroEntry()
    {
        var sut = new MacroPropertiesUpdateCommand
        {
            Id = 0,
            IsUsed = false,
            HasUnsupportedOps = false,
            Name = "Name",
            Description = "Description"
        };

        var state = new AtemState();
        var ex = Assert.Throws<IndexOutOfRangeException>(() => sut.ApplyToState(state));
        Assert.That(ex.Message, Contains.Substring("Macro ID"));
    }

}
