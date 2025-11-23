using AtemSharp.Commands.Fairlight.AudioRouting;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Tests.Commands.Fairlight.AudioRouting;

public class AudioRoutingSourceUpdateCommandTests : DeserializedCommandTestBase<AudioRoutingSourceUpdateCommand, AudioRoutingSourceUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint Id { get; set; }
        public uint SourceId { get; set; }
        public ushort ExternalPortType { get; set; }
        public ushort InternalPortType { get; set; }
        public string Name { get; set; } = string.Empty;
        public ushort AudioInputId { get; set; }
        public AudioChannelPair AudioChannelPair { get; set; }
    }

    protected override void CompareCommandProperties(AudioRoutingSourceUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
    {

    }
}
