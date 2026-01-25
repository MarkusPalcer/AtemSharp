using Argon;
using AtemSharp.Commands.Macro;
using AtemSharp.State.Macro;
using JetBrains.Annotations;
using NSubstitute;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroRecordCommandTests : SerializedCommandTestBase<MacroRecordCommand>
{
    [UsedImplicitly]
    private record CommandCreationParameters(ushort MacroId);

    protected override MacroRecordCommand CreateCommand(IStateHolder state, JObject? creationParameters)
    {
        var parameters = creationParameters!.ToObject<CommandCreationParameters>()!;
        return new MacroRecordCommand(state.Macros[parameters.MacroId]);
    }

    [Test]
    public void NewMacroId_ReplacedOldCommand()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = new MacroRecordCommand(state[2]) { Name = "First", Description = "First Macro" };
        var second = new MacroRecordCommand(state[3]) { Name = "Second", Description = "Second Macro" };

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

    static MacroRecordCommand Factory()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);
        return new MacroRecordCommand(state[2]);
    }
}
