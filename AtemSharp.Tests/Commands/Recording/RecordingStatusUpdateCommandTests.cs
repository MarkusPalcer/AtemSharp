using AtemSharp.Commands.Recording;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Recording;

internal class RecordingStatusUpdateCommandTests : DeserializedCommandTestBase<RecordingStatusUpdateCommand, RecordingStatusUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Status { get; set; }
        public ushort Error { get; set; }
        public uint TotalRecordingTimeAvailable { get; set; }
    }

    internal override void CompareCommandProperties(RecordingStatusUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That((ushort)actualCommand.Error, Is.EqualTo(expectedData.Error));
        Assert.That((ushort)actualCommand.Status, Is.EqualTo(expectedData.Status));
        Assert.That(actualCommand.RecordingTimeAvailable, Is.EqualTo(expectedData.TotalRecordingTimeAvailable));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Recording;
        Assert.That((ushort)actualCommand.Error, Is.EqualTo(expectedData.Error));
        Assert.That((ushort)actualCommand.Status, Is.EqualTo(expectedData.Status));
        Assert.That(actualCommand.RecordingTimeAvailable, Is.EqualTo(expectedData.TotalRecordingTimeAvailable));
    }
}
