using AtemSharp.Commands.Streaming;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Streaming;

public class StreamingStatusUpdateCommandTests : DeserializedCommandTestBase<StreamingStatusUpdateCommand, StreamingStatusUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Status { get; set; }
        public ushort Error { get; set; }
    }

    protected override void CompareCommandProperties(StreamingStatusUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That((ushort)actualCommand.Status, Is.EqualTo(expectedData.Status));
        Assert.That((ushort)actualCommand.Error, Is.EqualTo(expectedData.Error));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Streaming;
        Assert.That((ushort)actualCommand.Status, Is.EqualTo(expectedData.Status));
        Assert.That((ushort)actualCommand.Error, Is.EqualTo(expectedData.Error));
    }
}
