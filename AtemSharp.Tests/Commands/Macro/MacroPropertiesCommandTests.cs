using AtemSharp.Commands.Macro;
using AtemSharp.State.Macro;
using NSubstitute;

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
        var macro = new AtemSharp.State.Macro.Macro(Substitute.For<IAtemSwitcher>())
        {
            Id = testCase.Command.Index,
        };

        macro.UpdateName(testCase.Command.Name);
        macro.UpdateDescription(testCase.Command.Description);

        return new MacroPropertiesCommand(macro);
    }

    [Test]
    public void DoesNotMergeIfIndexIsDifferent()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);
        state[2].UpdateDescription("Desc1");
        state[2].UpdateName("Name1");
        state[3].UpdateDescription("Desc2");
        state[3].UpdateName("Name2");

        var first = new MacroPropertiesCommand(state[2]);
        var second = new  MacroPropertiesCommand(state[3]);

        Assert.That(second.TryMergeTo(first), Is.False);
        Assert.Multiple(() =>
        {
            Assert.That(first.Id, Is.EqualTo(2));
            Assert.That(second.Id, Is.EqualTo(3));
            Assert.That(first.Name, Is.EqualTo("Name1"));
            Assert.That(second.Name, Is.EqualTo("Name2"));
            Assert.That(first.Description, Is.EqualTo("Desc1"));
            Assert.That(second.Description, Is.EqualTo("Desc2"));
        });
    }

    [Test]
    public void MergeOnlyWhenChanged()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);

        var first = new MacroPropertiesCommand(state[2])
        {
            Name = "Name1",
            Description = "Desc1"
        };
        var second = new  MacroPropertiesCommand(state[2])
        {
            Name = "Name2"
        };

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.Multiple(() =>
        {
            Assert.That(first.Id, Is.EqualTo(2));
            Assert.That(first.Name, Is.EqualTo("Name2"));
            Assert.That(first.Description, Is.EqualTo("Desc1"));
        });
    }

    public static IEnumerable<TestCaseData> PropertyMergeTestCases()
    {
        yield return CreateMergingTestCase(nameof(MacroPropertiesCommand.Name), "Name1", "Name2");
        yield return CreateMergingTestCase(nameof(MacroPropertiesCommand.Description), "Desc1", "Desc2");
    }

    static MacroPropertiesCommand Factory()
    {
        var state = new MacroSystem(Substitute.For<IAtemSwitcher>());
        state.Populate(5);
        return new MacroPropertiesCommand(state[2]);
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
}
