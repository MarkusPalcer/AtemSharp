using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;
using AudioRoutingOutputUpdateCommand = AtemSharp.Commands.Audio.Fairlight.AudioRouting.AudioRoutingOutputUpdateCommand;

namespace AtemSharp.Tests.Commands.Fairlight.AudioRouting;

public class AudioRoutingOutputUpdateCommandTests : DeserializedCommandTestBase<AudioRoutingOutputUpdateCommand,
    AudioRoutingOutputUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public uint Id { get; set; }
        public uint SourceId { get; set; }
        public ushort ExternalPortType { get; set; }
        public ushort InternalPortType { get; set; }
        public string Name { get; set; } = string.Empty;
        public ushort AudioOutputId { get; set; }
        public AudioChannelPair AudioChannelPair { get; set; }
    }

    protected override void CompareCommandProperties(AudioRoutingOutputUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Id, Is.EqualTo(expectedData.Id));
        Assert.That(actualCommand.SourceId, Is.EqualTo(expectedData.SourceId));
        Assert.That(actualCommand.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(actualCommand.InternalPortType, Is.EqualTo(expectedData.InternalPortType));
        Assert.That(actualCommand.Name, Is.EqualTo(expectedData.Name));
        Assert.That(actualCommand.AudioOutputId, Is.EqualTo(expectedData.AudioOutputId));
        Assert.That(actualCommand.AudioChannelPair, Is.EqualTo(expectedData.AudioChannelPair));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new FairlightAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        var target = state.GetFairlight().AudioRouting.Outputs[expectedData.Id];
        Assert.That(target.Id, Is.EqualTo(expectedData.AudioOutputId));
        Assert.That(target.ExternalPortType, Is.EqualTo(expectedData.ExternalPortType));
        Assert.That(target.InternalPortType, Is.EqualTo(expectedData.InternalPortType));
        Assert.That(target.Name, Is.EqualTo(expectedData.Name));
        Assert.That(target.ChannelPair, Is.EqualTo(expectedData.AudioChannelPair));
    }
}
