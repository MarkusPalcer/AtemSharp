using AtemSharp.Commands.Recording;

namespace AtemSharp.Tests.Commands.Recording;

public class RecordingStatusUpdateCommandTests : DeserializedCommandTestBase<RecordingStatusUpdateCommand, RecordingStatusUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Status { get; set; }
        public ushort Error { get; set; }
        public uint TotalRecordingTimeAvailable { get; set; }
    }

    protected override void CompareCommandProperties(RecordingStatusUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That((ushort)actualCommand.Error, Is.EqualTo(expectedData.Error));
        Assert.That((ushort)actualCommand.Status, Is.EqualTo(expectedData.Status));
        Assert.That(actualCommand.RecordingTimeAvailable, Is.EqualTo(expectedData.TotalRecordingTimeAvailable));
    }
}
