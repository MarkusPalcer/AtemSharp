using AtemSharp.Commands3.Audio;

namespace AtemSharp.Tests.Commands3;

[TestFixture]
public class AudioMixerHeadphonesUpdateCommandTests : DeserializedCommandTestBase<AudioMixerHeadphonesUpdateCommand,
	AudioMixerHeadphonesUpdateCommandTests.CommandData>
{
	/// <summary>
	/// All properties in CAMH are floating-point decibel values
	/// </summary>
	protected override string[] GetFloatingPointProperties()
	{
		return new[] { "Gain", "ProgramOutGain", "TalkbackGain", "SidetoneGain" };
	}

	public class CommandData : CommandDataBase
	{
		public double Gain { get; set; }
		public double ProgramOutGain { get; set; }
		public double TalkbackGain { get; set; }
		public double SidetoneGain { get; set; }
	}

	protected override void CompareCommandProperties(AudioMixerHeadphonesUpdateCommand actualCommand, CommandData expectedData, TestCaseData testCase)
	{
		var floatingPointProps = GetFloatingPointProperties();
		var failures = new List<string>();

		// Compare Gain
		if (floatingPointProps.Contains("Gain"))
		{
			if (!AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain))
			{
				failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
			}
		}
		else if (!actualCommand.Gain.Equals(expectedData.Gain))
		{
			failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
		}

		// Compare ProgramOutGain
		if (floatingPointProps.Contains("ProgramOutGain"))
		{
			if (!AreApproximatelyEqual(actualCommand.ProgramOutGain, expectedData.ProgramOutGain))
			{
				failures.Add($"ProgramOutGain: expected {expectedData.ProgramOutGain}, actual {actualCommand.ProgramOutGain}");
			}
		}
		else if (!actualCommand.ProgramOutGain.Equals(expectedData.ProgramOutGain))
		{
			failures.Add($"ProgramOutGain: expected {expectedData.ProgramOutGain}, actual {actualCommand.ProgramOutGain}");
		}

		// Compare TalkbackGain
		if (floatingPointProps.Contains("TalkbackGain"))
		{
			if (!AreApproximatelyEqual(actualCommand.TalkbackGain, expectedData.TalkbackGain))
			{
				failures.Add($"TalkbackGain: expected {expectedData.TalkbackGain}, actual {actualCommand.TalkbackGain}");
			}
		}
		else if (!actualCommand.TalkbackGain.Equals(expectedData.TalkbackGain))
		{
			failures.Add($"TalkbackGain: expected {expectedData.TalkbackGain}, actual {actualCommand.TalkbackGain}");
		}

		// Compare SidetoneGain
		if (floatingPointProps.Contains("SidetoneGain"))
		{
			if (!AreApproximatelyEqual(actualCommand.SidetoneGain, expectedData.SidetoneGain))
			{
				failures.Add($"SidetoneGain: expected {expectedData.SidetoneGain}, actual {actualCommand.SidetoneGain}");
			}
		}
		else if (!actualCommand.SidetoneGain.Equals(expectedData.SidetoneGain))
		{
			failures.Add($"SidetoneGain: expected {expectedData.SidetoneGain}, actual {actualCommand.SidetoneGain}");
		}

		// Assert
		if (failures.Count > 0)
		{
			Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
			            string.Join("\n", failures));
		}
		
		Assert.Pass($"All properties match for version {testCase.FirstVersion}");
	}
}