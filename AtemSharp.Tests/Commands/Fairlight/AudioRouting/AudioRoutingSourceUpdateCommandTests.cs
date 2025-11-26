using AtemSharp.State.Audio.Fairlight;
using AudioRoutingSourceUpdateCommand = AtemSharp.Commands.Audio.Fairlight.AudioRouting.AudioRoutingSourceUpdateCommand;

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
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Id));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(actualCommand.InternalPortType, Is.EqualTo(expectedData.InternalPortType));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.AudioSourceId, Is.EqualTo(expectedData.AudioInputId));
        Assert.That(actualCommand.AudioChannelPair, Is.EqualTo(expectedData.AudioChannelPair));
    }
}
