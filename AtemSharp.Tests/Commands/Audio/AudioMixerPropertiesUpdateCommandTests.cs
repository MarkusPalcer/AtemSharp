using AudioMixerPropertiesUpdateCommand = AtemSharp.Commands.Audio.ClassicAudio.AudioMixerPropertiesUpdateCommand;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerPropertiesUpdateCommandTests : DeserializedCommandTestBase<AudioMixerPropertiesUpdateCommand,
	AudioMixerPropertiesUpdateCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public bool AudioFollowVideo { get; set; }
	}

	protected override void CompareCommandProperties(AudioMixerPropertiesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
        Assert.That(actualCommand.AudioFollowVideo, Is.EqualTo(expectedData.AudioFollowVideo));
	}
}
