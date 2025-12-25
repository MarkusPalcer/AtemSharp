using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.Tests.TestUtilities;

namespace AtemSharp.Tests.Commands.DeviceProfile;

internal class TopologyCommandTests : DeserializedCommandTestBase<TopologyCommand, TopologyCommandTests.CommandData>
{
    [MaxProtocolVersion(ProtocolVersion.V7_5_2)]
    public class CommandData : CommandDataBase
    {
        public int MixEffectBlocks { get; set; }
        public int VideoSources { get; set; }
        public int DownstreamKeyers { get; set; }
        public int Auxiliaries { get; set; }
        public int MixMinusOutputs { get; set; }
        public int MediaPlayers { get; set; }
        public int Multiviewers { get; set; } = -1;
        public int SerialPort { get; set; }
        public int HyperDecks { get; set; }

        // ReSharper disable once InconsistentNaming - Domain Specific Acronym
        public int DVE { get; set; }
        public int Stingers { get; set; }
        public int SuperSource { get; set; }
        public int TalkbackChannels { get; set; }
        public bool CameraControl { get; set; }
        public bool AdvancedChromaKeyers { get; set; }
        public bool OnlyConfigurableOutputs { get; set; }
    }

    internal override void CompareCommandProperties(TopologyCommand actualCommand, CommandData expectedData)
    {
        Assert.That(actualCommand.MixEffects, Is.EqualTo(expectedData.MixEffectBlocks));
        Assert.That(actualCommand.Sources, Is.EqualTo(expectedData.VideoSources));
        Assert.That(actualCommand.DownstreamKeyers, Is.EqualTo(expectedData.DownstreamKeyers));
        Assert.That(actualCommand.Auxiliaries, Is.EqualTo(expectedData.Auxiliaries));
        Assert.That(actualCommand.MixMinusOutputs, Is.EqualTo(expectedData.MixMinusOutputs));
        Assert.That(actualCommand.MediaPlayers, Is.EqualTo(expectedData.MediaPlayers));
        Assert.That(actualCommand.SerialPorts, Is.EqualTo(expectedData.SerialPort));
        Assert.That(actualCommand.MaxHyperdecks, Is.EqualTo(expectedData.HyperDecks));
        Assert.That(actualCommand.DigitalVideoEffects, Is.EqualTo(expectedData.DVE));
        Assert.That(actualCommand.Stingers, Is.EqualTo(expectedData.Stingers));
        Assert.That(actualCommand.SuperSources, Is.EqualTo(expectedData.SuperSource));
        Assert.That(actualCommand.TalkbackChannels, Is.EqualTo(expectedData.TalkbackChannels));
        Assert.That(actualCommand.CameraControl, Is.EqualTo(expectedData.CameraControl));
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.Info.Capabilities.MixEffects, Is.EqualTo(expectedData.MixEffectBlocks));
        Assert.That(state.Info.Capabilities.Sources, Is.EqualTo(expectedData.VideoSources));
        Assert.That(state.Info.Capabilities.DownstreamKeyers, Is.EqualTo(expectedData.DownstreamKeyers));
        Assert.That(state.Info.Capabilities.Auxiliaries, Is.EqualTo(expectedData.Auxiliaries));
        Assert.That(state.Info.Capabilities.MixMinusOutputs, Is.EqualTo(expectedData.MixMinusOutputs));
        Assert.That(state.Info.Capabilities.MediaPlayers, Is.EqualTo(expectedData.MediaPlayers));
        Assert.That(state.Info.Capabilities.SerialPorts, Is.EqualTo(expectedData.SerialPort));
        Assert.That(state.Info.Capabilities.MaxHyperdecks, Is.EqualTo(expectedData.HyperDecks));
        Assert.That(state.Info.Capabilities.DigitalVideoEffects, Is.EqualTo(expectedData.DVE));
        Assert.That(state.Info.Capabilities.Stingers, Is.EqualTo(expectedData.Stingers));
        Assert.That(state.Info.Capabilities.SuperSources, Is.EqualTo(expectedData.SuperSource));
        Assert.That(state.Info.Capabilities.TalkbackChannels, Is.EqualTo(expectedData.TalkbackChannels));
        Assert.That(state.Info.Capabilities.CameraControl, Is.EqualTo(expectedData.CameraControl));
        Assert.That(state.Info.Capabilities.AdvancedChromaKeyers, Is.False);
        Assert.That(state.Info.Capabilities.OnlyConfigurableOutputs, Is.False);

        Assert.That(state.Info.MixEffects.Count(), Is.EqualTo(expectedData.MixEffectBlocks));
        Assert.That(state.Video.MixEffects.Count(), Is.EqualTo(expectedData.MixEffectBlocks));
        Assert.That(state.Video.Auxiliaries.Count(), Is.EqualTo(expectedData.Auxiliaries));
        Assert.That(state.Media.Players.Count(), Is.EqualTo(expectedData.MediaPlayers));
        Assert.That(state.Info.SuperSources.Count(), Is.EqualTo(expectedData.SuperSource));
        Assert.That(state.Video.SuperSources.Count(), Is.EqualTo(expectedData.SuperSource));
        Assert.That(state.Video.DownstreamKeyers.Count(), Is.EqualTo(expectedData.DownstreamKeyers));
    }
}
