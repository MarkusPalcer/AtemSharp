using AtemSharp.Commands.Recording;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Recording;

internal class RecordingIsoUpdateCommandTests : DeserializedCommandTestBase<RecordingIsoUpdateCommand, RecordingIsoUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsoRecordAllInputs { get; set; }
    }

    internal override void CompareCommandProperties(RecordingIsoUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.RecordAllInputs, Is.EqualTo(expectedData.IsoRecordAllInputs));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Recording.RecordAllInputs, Is.EqualTo(expectedData.IsoRecordAllInputs));
    }
}
