using AtemSharp.Commands.Macro;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Macro;

internal class MacroRecordingStatusCommandTests : DeserializedCommandTestBase<MacroRecordingStatusCommand,
    MacroRecordingStatusCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsRecording { get; set; }
        public ushort Index { get; set; }
    }

    internal override void CompareCommandProperties(MacroRecordingStatusCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.IsRecording, Is.EqualTo(expectedData.IsRecording));
        Assert.That(actualCommand.MacroIndex, Is.EqualTo(expectedData.Index));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Macros.Recorder;
        Assert.That(actualCommand.IsRecording, Is.EqualTo(expectedData.IsRecording));
        Assert.That(actualCommand.MacroIndex, Is.EqualTo(expectedData.Index));
    }
}
