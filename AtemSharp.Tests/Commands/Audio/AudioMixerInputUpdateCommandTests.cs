using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AtemSharp.State.Ports;
using AudioMixerInputUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerInputUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
internal class AudioMixerInputUpdateCommandTests : DeserializedCommandTestBase<AudioMixerInputUpdateCommand,
    AudioMixerInputUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public ushort Index { get; set; }
        public AudioSourceType SourceType { get; set; }
        public ExternalPortType PortType { get; set; }
        public AudioMixOption MixOption { get; set; }
        public double Gain { get; set; }
        public double Balance { get; set; }

        public bool IndexOfSourceType { get; set; }

        public bool SupportsRcaToXlrEnabled { get; set; }

        public bool RcaToXlrEnabled { get; set; }
    }

    internal override void CompareCommandProperties(AudioMixerInputUpdateCommand actualCommand, CommandData expectedData, TestUtilities.CommandTests.TestCaseData<CommandData> testCase)
    {
        Assert.That(actualCommand.Index, Is.EqualTo(expectedData.Index));
        Assert.That(actualCommand.SourceType, Is.EqualTo(expectedData.SourceType));
        Assert.That(actualCommand.PortType, Is.EqualTo(expectedData.PortType));
        Assert.That(actualCommand.MixOption, Is.EqualTo(expectedData.MixOption));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.Balance, Is.EqualTo(expectedData.Balance).Within(0.01));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new ClassicAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].Id, Is.EqualTo(expectedData.Index));
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].SourceType, Is.EqualTo(expectedData.SourceType));
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].PortType, Is.EqualTo(expectedData.PortType));
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].MixOption, Is.EqualTo(expectedData.MixOption));
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].Balance, Is.EqualTo(expectedData.Balance).Within(0.01));
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].SupportsRcaToXlrEnabled, Is.False);
        Assert.That(state.GetClassicAudio().Channels[expectedData.Index].RcaToXlrEnabled, Is.False);
    }
}
