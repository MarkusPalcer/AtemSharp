using System.Text;
using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands3.Audio;

/// <summary>
/// Command to update audio mixer headphones properties
/// </summary>
[Command("CAMH")]
public class AudioMixerHeadphonesCommand : WritableCommand<AudioMixerHeadphonesCommand>
{
	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	[CommandProperty(1 << 0, 0)]
	public double? Gain { get; set; }

	/// <summary>
	/// Program out gain in decibel, -Infinity to +6dB
	/// </summary>
	[CommandProperty(1 << 1, 1)]
	public double? ProgramOutGain { get; set; }

	/// <summary>
	/// Talkback gain in decibel, -Infinity to +6dB
	/// </summary>
	[CommandProperty(1 << 2, 2)]
	public double? TalkbackGain { get; set; }

	/// <summary>
	/// Sidetone gain in decibel, -Infinity to +6dB
	/// </summary>
	[CommandProperty(1 << 3, 3)]
	public double? SidetoneGain { get; set; }

	// Internal properties that hold the actual values to write (like TypeScript this.properties)
	public double ActualGain { get; set; } = 0.0;
	public double ActualProgramOutGain { get; set; } = 0.0;
	public double ActualTalkbackGain { get; set; } = 0.0;
	public double ActualSidetoneGain { get; set; } = 0.0;

	/// <inheritdoc />
	public override Stream Serialize(ProtocolVersion version)
	{
		var buffer = new byte[12];
		
		// Calculate flag based on which properties are explicitly set (not null)
		byte flag = 0;
		if (Gain.HasValue) flag |= 1 << 0;
		if (ProgramOutGain.HasValue) flag |= 1 << 1;
		if (TalkbackGain.HasValue) flag |= 1 << 2;
		if (SidetoneGain.HasValue) flag |= 1 << 3;

		// Flag at index 0
		buffer[0] = flag;
		// Index 1 is skipped (not written to, remains 0)

		// Always write all actual values (like TypeScript: this.properties.gain || 0)
		// Use the actual values for serialization, not the nullable properties
		var gainValue = AtemUtil.DecibelToUInt16BE(Gain ?? ActualGain);
		buffer[2] = (byte)(gainValue >> 8);
		buffer[3] = (byte)(gainValue & 0xFF);

		var programOutGainValue = AtemUtil.DecibelToUInt16BE(ProgramOutGain ?? ActualProgramOutGain);
		buffer[4] = (byte)(programOutGainValue >> 8);
		buffer[5] = (byte)(programOutGainValue & 0xFF);

		var talkbackGainValue = AtemUtil.DecibelToUInt16BE(TalkbackGain ?? ActualTalkbackGain);
		buffer[6] = (byte)(talkbackGainValue >> 8);
		buffer[7] = (byte)(talkbackGainValue & 0xFF);

		var sidetoneGainValue = AtemUtil.DecibelToUInt16BE(SidetoneGain ?? ActualSidetoneGain);
		buffer[8] = (byte)(sidetoneGainValue >> 8);
		buffer[9] = (byte)(sidetoneGainValue & 0xFF);

		// Indices 10-11 remain 0 (padding)

		return new MemoryStream(buffer);
	}
}