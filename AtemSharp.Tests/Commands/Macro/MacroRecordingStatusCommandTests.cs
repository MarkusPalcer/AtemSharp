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

    internal override void CompareCommandProperties(MacroRecordingStatusCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.IsRecording, Is.EqualTo(expectedData.IsRecording));
        Assert.That(actualCommand.MacroIndex, Is.EqualTo(expectedData.Index));
    }

    protected override void PrepareState(IStateHolder stateHolder, CommandData testData)
    {
        stateHolder.Macros.GetOrCreate(testData.Index);
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        throw new NotImplementedException();
    }

    protected override void CompareStateProperties(IStateHolder state, CommandData expectedData)
    {
        if (expectedData.IsRecording)
        {
            Assert.That(state.Macros.CurrentlyRecording, Is.SameAs(state.Macros[expectedData.Index]));
        }
        else
        {
            Assert.That(state.Macros.CurrentlyRecording, Is.Null);
        }
    }
}
