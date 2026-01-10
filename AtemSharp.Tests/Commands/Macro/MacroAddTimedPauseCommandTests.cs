using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroAddTimedPauseCommandTests : SerializedCommandTestBase<MacroAddTimedPauseCommand,
    MacroAddTimedPauseCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Frames { get; set; }
    }

    protected override MacroAddTimedPauseCommand CreateSut(TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        return new MacroAddTimedPauseCommand
        {
            Frames = testCase.Command.Frames
        };
    }

    [Test]
    public void MergeCommand()
    {
        var first = new MacroAddTimedPauseCommand { Frames = 2 };
        var second = new MacroAddTimedPauseCommand { Frames = 3 };

        Assert.That(second.TryMergeTo(first), Is.True);
        Assert.That(first.Frames, Is.EqualTo(5));
    }
}
