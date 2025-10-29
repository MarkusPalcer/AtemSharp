using AtemSharp.Commands.Streaming;

namespace AtemSharp.Tests.Commands.Streaming;

public class StreamingAudioBitratesUpdateCommandTests : DeserializedCommandTestBase<StreamingAudioBitratesUpdateCommand, StreamingAudioBitratesUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint LowBitrate { get; set; }
        public uint HighBitrate { get; set; }
    }

    protected override void CompareCommandProperties(StreamingAudioBitratesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {
        Assert.That(actualCommand.LowBitrate, Is.EqualTo(expectedData.LowBitrate));
        Assert.That(actualCommand.HighBitrate, Is.EqualTo(expectedData.HighBitrate));
    }
}
