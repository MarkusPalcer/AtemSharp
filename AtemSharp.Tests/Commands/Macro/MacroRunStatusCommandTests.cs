using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroRunStatusCommandTests : SerializedCommandTestBase<MacroRunStatusCommand, MacroRunStatusCommandTests.CommandData> {
    public class CommandData : CommandDataBase
    {
        public bool Loop { get; set; }
    }

    protected override MacroRunStatusCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MacroRunStatusCommand
        {
            Loop = testCase.Command.Loop
        };
    }

    [Test]
    [TestCase(false, false, false)]
    [TestCase(true, false, false)]
    [TestCase(true, true, true)]
    [TestCase(false, true, true)]
    public void NewCommandOverridesOldValue(bool first, bool second, bool expected)
    {
        var firstCommand = new MacroRunStatusCommand { Loop = first };
        var secondCommand = new MacroRunStatusCommand { Loop = second };

        Assert.That(secondCommand.TryMergeTo(firstCommand), Is.True);
        Assert.That(firstCommand.Loop, Is.EqualTo(expected));
    }

    [Test]
    public void TestPropertyMerging_WithWrongType()
    {
        TestPropertyMerging_WithWrongType(() => new MacroRunStatusCommand());
    }
}
