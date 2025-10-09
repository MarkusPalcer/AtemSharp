using AtemSharp.Commands.Audio;
using AtemSharp.Enums.Audio;
using AtemSharp.Enums.Ports;

namespace AtemSharp.Tests.Commands.Audio;

[TestFixture]
public class AudioMixerInputUpdateV8CommandTests : DeserializedCommandTestBase<AudioMixerInputUpdateV8Command,
	AudioMixerInputUpdateV8CommandTests.CommandData>
{
	public class CommandData : CommandDataBase
	{
		public ushort Index { get; set; }
		public AudioSourceType SourceType { get; set; }
		public ExternalPortType PortType { get; set; }
		public AudioMixOption MixOption { get; set; }
		public double Gain { get; set; }
		public double Balance { get; set; }
		public bool SupportsRcaToXlrEnabled { get; set; }
		public bool RcaToXlrEnabled { get; set; }
	}

	protected override void CompareCommandProperties(AudioMixerInputUpdateV8Command actualCommand, CommandData expectedData, TestCaseData testCase)
	{
		var failures = new List<string>();

		// Compare Index
		if (actualCommand.Index != expectedData.Index)
		{
			failures.Add($"Index: expected {expectedData.Index}, actual {actualCommand.Index}");
		}

		// Compare SourceType
		if (actualCommand.SourceType != expectedData.SourceType)
		{
			failures.Add($"SourceType: expected {expectedData.SourceType}, actual {actualCommand.SourceType}");
		}

		// Compare PortType
		if (actualCommand.PortType != expectedData.PortType)
		{
			failures.Add($"PortType: expected {expectedData.PortType}, actual {actualCommand.PortType}");
		}

		// Compare MixOption
		if (actualCommand.MixOption != expectedData.MixOption)
		{
			failures.Add($"MixOption: expected {expectedData.MixOption}, actual {actualCommand.MixOption}");
		}

		// Compare Gain (floating point)
		if (!AreApproximatelyEqual(actualCommand.Gain, expectedData.Gain))
		{
			failures.Add($"Gain: expected {expectedData.Gain}, actual {actualCommand.Gain}");
		}

		// Compare Balance (floating point)
		if (!AreApproximatelyEqual(actualCommand.Balance, expectedData.Balance))
		{
			failures.Add($"Balance: expected {expectedData.Balance}, actual {actualCommand.Balance}");
		}

		// Compare SupportsRcaToXlrEnabled
		if (actualCommand.SupportsRcaToXlrEnabled != expectedData.SupportsRcaToXlrEnabled)
		{
			failures.Add($"SupportsRcaToXlrEnabled: expected {expectedData.SupportsRcaToXlrEnabled}, actual {actualCommand.SupportsRcaToXlrEnabled}");
		}

		// Compare RcaToXlrEnabled
		if (actualCommand.RcaToXlrEnabled != expectedData.RcaToXlrEnabled)
		{
			failures.Add($"RcaToXlrEnabled: expected {expectedData.RcaToXlrEnabled}, actual {actualCommand.RcaToXlrEnabled}");
		}

		// Assert
		if (failures.Count > 0)
		{
			Assert.Fail($"Command deserialization property mismatch for version {testCase.FirstVersion}:\n" +
			            string.Join("\n", failures));
		}
	}
}