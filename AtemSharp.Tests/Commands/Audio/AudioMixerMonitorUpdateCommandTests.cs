using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;
using AudioMixerMonitorUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerMonitorUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
internal class AudioMixerMonitorUpdateCommandTests : DeserializedCommandTestBase<AudioMixerMonitorUpdateCommand,
    AudioMixerMonitorUpdateCommandTests.CommandData>
{
    public class CommandData : CommandDataBase
    {
        public bool Enabled { get; set; }
        public double Gain { get; set; }
        public bool Mute { get; set; }
        public bool Solo { get; set; }
        public int SoloSource { get; set; }
        public bool Dim { get; set; }
        public double DimLevel { get; set; }
    }

    internal override void CompareCommandProperties(AudioMixerMonitorUpdateCommand actualCommand, CommandData expectedData,
                                                     TestCaseData testCase)
    {
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.Mute, Is.EqualTo(expectedData.Mute));
        Assert.That(actualCommand.Solo, Is.EqualTo(expectedData.Solo));
        Assert.That(actualCommand.SoloSource, Is.EqualTo(expectedData.SoloSource));
        Assert.That(actualCommand.Dim, Is.EqualTo(expectedData.Dim));
        Assert.That(actualCommand.DimLevel, Is.EqualTo(expectedData.DimLevel).Within(0.01));
    }

    protected override void PrepareState(AtemState state, CommandData expectedData)
    {
        state.Audio = new ClassicAudioState();
    }

    protected override void CompareStateProperties(AtemState state, CommandData expectedData)
    {
        Assert.That(state.GetClassicAudio().Monitor.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(state.GetClassicAudio().Monitor.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(state.GetClassicAudio().Monitor.Mute, Is.EqualTo(expectedData.Mute));
        Assert.That(state.GetClassicAudio().Monitor.Solo, Is.EqualTo(expectedData.Solo));
        Assert.That(state.GetClassicAudio().Monitor.SoloSource, Is.EqualTo(expectedData.SoloSource));
        Assert.That(state.GetClassicAudio().Monitor.Dim, Is.EqualTo(expectedData.Dim));
        Assert.That(state.GetClassicAudio().Monitor.DimLevel, Is.EqualTo(expectedData.DimLevel).Within(0.01));
    }
}
