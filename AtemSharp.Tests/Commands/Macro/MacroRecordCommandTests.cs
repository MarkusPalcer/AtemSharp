using AtemSharp.Commands.Macro;
using AtemSharp.State.Macro;
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

    static MacroRecordCommand Factory()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);
        return new MacroRecordCommand(state[2]);
    }

    [Test]
    public void NewMacroId_ReplacedOldCommand()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = new  MacroRecordCommand(state[2]) { Name = "First" ,  Description = "First Macro" };
        var second = new  MacroRecordCommand(state[3]) { Name = "Second" ,  Description = "Second Macro" };

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.That(first.Index, Is.EqualTo(3));
        Assert.That(first.Name, Is.EqualTo("Second"));
        Assert.That(first.Description, Is.EqualTo("Second Macro"));
    }

    public static IEnumerable<TestCaseData> PropertyMergeTestCases()
    {
        yield return CreateMergingTestCase(nameof(MacroRecordCommand.Name), "Name1", "Name2");
        yield return CreateMergingTestCase(nameof(MacroRecordCommand.Description), "Desc1", "Desc2");
    }

    [Test]
    [TestCaseSource(nameof(PropertyMergeTestCases))]
    public void IfPropertyChangedOnNewCommand_ItIsChangedOnOldCommand(string property, object firstValue, object secondValue)
    {
        TestPropertyMerging(Factory, property, firstValue, secondValue);
    }

    [Test]
    [TestCaseSource(nameof(PropertyMergeTestCases))]
    public void IfPropertyIsUnchangedOnNewCommand_ItRetainsTheValueOfTheOldCommand(string property, object firstValue, object secondValue)
    {
        TestPropertyNonMerging(Factory, property, firstValue, secondValue);
    }

    [Test]
    public void TestPropertyMerging_WithWrongType()
    {
        TestPropertyMerging_WithWrongType(Factory);
    }
}
