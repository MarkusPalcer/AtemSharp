using AtemSharp.Commands.Recording;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingIsoUpdateCommandTests : DeserializedCommandTestBase<RecordingIsoUpdateCommand, RecordingIsoUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool IsoRecordAllInputs { get; set; }
    }

    protected override void CompareCommandProperties(RecordingIsoUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.RecordAllInputs, Is.EqualTo(expectedData.IsoRecordAllInputs));
    }
}
