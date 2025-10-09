using AtemSharp.Commands.Audio;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerHeadphonesUpdateCommandTests : DeserializedCommandTestBase<AudioMixerHeadphonesUpdateCommand,
	AudioMixerHeadphonesUpdateCommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public double Gain { get; set; }
		public double ProgramOutGain { get; set; }
		public double TalkbackGain { get; set; }
		public double SidetoneGain { get; set; }
	}

	protected override void CompareCommandProperties(AudioMixerHeadphonesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
		var failures = new List<string>();

		// Compare Gain
		if (!AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain))
		{
			failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
		}

		// Compare ProgramOutGain
		if (!AreApproximatelyEqual(actualCommand.ProgramOutGain, expectedData.ProgramOutGain))
		{
			failures.Add($"ProgramOutGain: expected {expectedData.ProgramOutGain}, actual {actualCommand.ProgramOutGain}");
		}

		// Compare TalkbackGain
		if (!AreApproximatelyEqual(actualCommand.TalkbackGain, expectedData.TalkbackGain))
		{
			failures.Add($"TalkbackGain: expected {expectedData.TalkbackGain}, actual {actualCommand.TalkbackGain}");
		}

		// Compare SidetoneGain
		if (!AreApproximatelyEqual(actualCommand.SidetoneGain, expectedData.SidetoneGain))
		{
			failures.Add($"SidetoneGain: expected {expectedData.SidetoneGain}, actual {actualCommand.SidetoneGain}");
		}

		// Assert
		if (failures.Count > 0)
		{
			Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
			            string.Join("\n", failures));
		}
	}
}