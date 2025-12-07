using AtemSharp.Commands.Streaming;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Streaming;

public class StreamingStatsUpdateCommandTests : DeserializedCommandTestBase<StreamingStatsUpdateCommand, StreamingStatsUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint EncodingBitrate { get; set; }
        public ushort CacheUsed { get; set; }
    }

    protected override void CompareCommandProperties(StreamingStatsUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.EncodingBitrate, Is.EqualTo(expectedData.EncodingBitrate));
        Assert.That(actualCommand.CacheUsed, Is.EqualTo(expectedData.CacheUsed));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var actualCommand = state.Streaming;
        Assert.That(actualCommand.EncodingBitrate, Is.EqualTo(expectedData.EncodingBitrate));
        Assert.That(actualCommand.CacheUsed, Is.EqualTo(expectedData.CacheUsed));
    }
}
