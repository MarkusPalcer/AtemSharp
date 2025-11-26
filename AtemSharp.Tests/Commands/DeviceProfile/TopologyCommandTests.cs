using AtemSharp.Commands.DeviceProfile;
using AtemSharp.State.Info;

namespace AtemSharp.Tests.Commands.DeviceProfile;

[TestFixture]
public class TopologyCommandTests : DeserializedCommandTestBase<TopologyCommand, TopologyCommandTests.CommandData>
{
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

    protected override void CompareCommandProperties(TopologyCommand actualCommand, CommandData expectedData, TestCaseData testCase)
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

        // Only compare extended properties for newer protocol versions (V8_0 and later)
        var isNewProtocol = testCase.FirstVersion >= ProtocolVersion.V8_0;

        if (isNewProtocol)
        {
            Assert.That(actualCommand.Multiviewers, Is.EqualTo(expectedData.Multiviewers));
            Assert.That(actualCommand.TalkbackChannels, Is.EqualTo(expectedData.TalkbackChannels));
            Assert.That(actualCommand.CameraControl, Is.EqualTo(expectedData.CameraControl));
            Assert.That(actualCommand.AdvancedChromaKeyers, Is.EqualTo(expectedData.AdvancedChromaKeyers));
            Assert.That(actualCommand.OnlyConfigurableOutputs, Is.EqualTo(expectedData.OnlyConfigurableOutputs));
        }
        else
        {
            Assert.That(actualCommand.Multiviewers, Is.EqualTo(-1));
            Assert.That(actualCommand.TalkbackChannels, Is.EqualTo(0));
            Assert.That(actualCommand.CameraControl, Is.False);
            Assert.That(actualCommand.AdvancedChromaKeyers, Is.False);
            Assert.That(actualCommand.OnlyConfigurableOutputs, Is.False);
        }
    }
}
