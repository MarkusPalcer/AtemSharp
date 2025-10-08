using AudioMixerHeadphonesCommand = AtemSharp.Commands3.Audio.AudioMixerHeadphonesCommand;

namespace AtemSharp.Tests.Commands3;

[TestFixture]
public class AudioMixerHeadphonesCommandTests : SerializedCommandTestBase<AudioMixerHeadphonesCommand,
	AudioMixerHeadphonesCommandTests.CommandData>
{
	/// <summary>
	/// CAMH command has floating-point encoded gain values at bytes 2-9
	/// (Gain: bytes 2-3, ProgramOutGain: bytes 4-5, TalkbackGain: bytes 6-7, SidetoneGain: bytes 8-9)
	/// </summary>
	protected override Range[] GetFloatingPointByteRanges()
	{
		return new[] { 2..10 }; // Using C# range syntax: bytes 2 through 9 (inclusive), end index is exclusive
	}

	public class CommandData : CommandDataBase
	{
		public double Gain { get; set; }
		public double ProgramOutGain { get; set; }
		public double TalkbackGain { get; set; }
		public double SidetoneGain { get; set; }
	}

	protected override AudioMixerHeadphonesCommand CreateSut(TestCaseData testCase)
	{
		// Create Commands3 command
		var command = new AudioMixerHeadphonesCommand();

		// Set the actual values that should be written (like TypeScript this.properties)
		command.ActualGain = testCase.Command.Gain;
		command.ActualProgramOutGain = testCase.Command.ProgramOutGain;
		command.ActualTalkbackGain = testCase.Command.TalkbackGain;
		command.ActualSidetoneGain = testCase.Command.SidetoneGain;

		// Set nullable properties only for those indicated by mask (for flag calculation)
		if ((testCase.Command.Mask & (1 << 0)) != 0) command.Gain = testCase.Command.Gain;
		if ((testCase.Command.Mask & (1 << 1)) != 0) command.ProgramOutGain = testCase.Command.ProgramOutGain;
		if ((testCase.Command.Mask & (1 << 2)) != 0) command.TalkbackGain = testCase.Command.TalkbackGain;
		if ((testCase.Command.Mask & (1 << 3)) != 0) command.SidetoneGain = testCase.Command.SidetoneGain;

		return command;
	}
}