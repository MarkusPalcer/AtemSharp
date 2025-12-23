using AtemSharp.Commands.Streaming;
using AtemSharp.State;

namespace AtemSharp.Tests.Commands.Streaming;

internal class StreamingServiceUpdateCommandTests : DeserializedCommandTestBase<StreamingServiceUpdateCommand, StreamingServiceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public string ServiceName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public uint[] Bitrates { get; set; } = [];
    }

    internal override void CompareCommandProperties(StreamingServiceUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.ServiceName, Is.EqualTo(expectedData.ServiceName));
        Assert.That(actualCommand.Url, Is.EqualTo(expectedData.Url));
        Assert.That(actualCommand.Key, Is.EqualTo(expectedData.Key));
        Assert.That(actualCommand.Bitrate1, Is.EqualTo(expectedData.Bitrates[0]));
        Assert.That(actualCommand.Bitrate2, Is.EqualTo(expectedData.Bitrates[1]));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var streaming = state.Streaming;
        Assert.That(streaming.ServiceName, Is.EqualTo(expectedData.ServiceName));
        Assert.That(streaming.Url, Is.EqualTo(expectedData.Url));
        Assert.That(streaming.Key, Is.EqualTo(expectedData.Key));
        Assert.That(streaming.VideoBitrates.Low, Is.EqualTo(expectedData.Bitrates[0]));
        Assert.That(streaming.VideoBitrates.High, Is.EqualTo(expectedData.Bitrates[1]));
    }
}
