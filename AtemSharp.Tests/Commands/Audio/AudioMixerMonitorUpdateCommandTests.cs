using AtemSharp.Commands.Audio;

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
		var failures = new List<string>();

		// Compare Enabled
		if (actualCommand.Enabled != expectedData.Enabled)
		{
			failures.Add($"Enabled: expected {expectedData.Enabled}, actual {actualCommand.Enabled}");
		}

		// Compare Gain
		if (!AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain))
		{
			failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
		}

		// Compare Mute
		if (actualCommand.Mute != expectedData.Mute)
		{
			failures.Add($"Mute: expected {expectedData.Mute}, actual {actualCommand.Mute}");
		}

		// Compare Solo
		if (actualCommand.Solo != expectedData.Solo)
		{
			failures.Add($"Solo: expected {expectedData.Solo}, actual {actualCommand.Solo}");
		}

		// Compare SoloSource
		if (actualCommand.SoloSource != expectedData.SoloSource)
		{
			failures.Add($"SoloSource: expected {expectedData.SoloSource}, actual {actualCommand.SoloSource}");
		}

		// Compare Dim
		if (actualCommand.Dim != expectedData.Dim)
		{
			failures.Add($"Dim: expected {expectedData.Dim}, actual {actualCommand.Dim}");
		}

		// Compare DimLevel
		if (!AreApproximatelyEqual(actualCommand.DimLevel, expectedData.DimLevel))
		{
			failures.Add($"DimLevel: expected {expectedData.DimLevel}, actual {actualCommand.DimLevel}");
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