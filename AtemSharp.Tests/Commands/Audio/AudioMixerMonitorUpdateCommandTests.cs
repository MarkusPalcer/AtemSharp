using AudioMixerMonitorUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerMonitorUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerMonitorUpdateCommandTests : DeserializedCommandTestBase<AudioMixerMonitorUpdateCommand,
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

	protected override void CompareCommandProperties(AudioMixerMonitorUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
        Assert.That(actualCommand.Enabled, Is.EqualTo(expectedData.Enabled));
        Assert.That(actualCommand.Gain, Is.EqualTo(expectedData.Gain).Within(0.01));
        Assert.That(actualCommand.Mute, Is.EqualTo(expectedData.Mute));
        Assert.That(actualCommand.Solo, Is.EqualTo(expectedData.Solo));
        Assert.That(actualCommand.SoloSource, Is.EqualTo(expectedData.SoloSource));
        Assert.That(actualCommand.Dim, Is.EqualTo(expectedData.Dim));
        Assert.That(actualCommand.DimLevel, Is.EqualTo(expectedData.DimLevel).Within(0.01));
	}
}
