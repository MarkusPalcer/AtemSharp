using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AudioRoutingSourceUpdateCommand = AtemSharp.Commands.Audio.Fairlight.AudioRouting.AudioRoutingSourceUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.AudioRouting;

internal class AudioRoutingSourceUpdateCommandTests : DeserializedCommandTestBase<AudioRoutingSourceUpdateCommand,
    AudioRoutingSourceUpdateCommandTests.CommandData>
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

    internal override void CompareCommandProperties(AudioRoutingSourceUpdateCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Id));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(actualCommand.InternalPortType, Is.EqualTo(expectedData.InternalPortType));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.AudioSourceId, Is.EqualTo(expectedData.AudioInputId));
        Assert.That(actualCommand.AudioChannelPair, Is.EqualTo(expectedData.AudioChannelPair));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.GetFairlight().AudioRouting.Sources[expectedData.Id];
        Assert.That(target.Id, Is.EqualTo(expectedData.AudioInputId));
        Assert.That(target.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(target.InternalPortType, Is.EqualTo(expectedData.InternalPortType));
        Assert.That(target.Name, Is.EqualTo(expectedData.Name));
        Assert.That(target.ChannelPair, Is.EqualTo(expectedData.AudioChannelPair));
    }
}
