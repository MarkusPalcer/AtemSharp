using AtemSharp.Commands.Macro;

namespace AtemSharp.Tests.Commands.Macro;

public class MacroRecordingStatusCommandTests : DeserializedCommandTestBase<MacroRecordingStatusCommand,
    MacroRecordingStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsRecording { get; set; }
        public ushort Index { get; set; }
    }

    protected override void CompareCommandProperties(MacroRecordingStatusCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.IsRecording, Is.EqualTo(testCase.Command.IsRecording));
        Assert.That(actualCommand.MacroIndex, Is.EqualTo(testCase.Command.Index));
    }
}
