using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

[Command("AMHP")]
public class AudioMixerHeadphonesUpdateCommand : IDeserializedCommand
{
	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain { get; init; }

	/// <summary>
	/// Program out gain in decibel, -Infinity to +6dB
	/// </summary>
	public double ProgramOutGain { get; init; }

	/// <summary>
	/// Sidetone gain in decibel, -Infinity to +6dB
	/// </summary>
	public double SidetoneGain { get; init; }

	/// <summary>
	/// Talkback gain in decibel, -Infinity to +6dB
	/// </summary>
	public double TalkbackGain { get; init; }

	public static AudioMixerHeadphonesUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
		return new AudioMixerHeadphonesUpdateCommand
		{
			Gain = rawCommand.ReadUInt16BigEndian(0).UInt16ToDecibel(),
			ProgramOutGain = rawCommand.ReadUInt16BigEndian(2).UInt16ToDecibel(),
			TalkbackGain = rawCommand.ReadUInt16BigEndian(4).UInt16ToDecibel(),
			SidetoneGain = rawCommand.ReadUInt16BigEndian(6).UInt16ToDecibel()
		};
	}

	/// <inheritdoc />
	public void ApplyToState(AtemState state)
	{
		if (state.Audio is null)
		{
			throw new InvalidIdError("Classic Audio", "headphones");
		}

		state.Audio.Headphones ??= new ClassicAudioHeadphoneOutputChannel();

		state.Audio.Headphones.Gain = Gain;
		state.Audio.Headphones.ProgramOutGain = ProgramOutGain;
		state.Audio.Headphones.TalkbackGain = TalkbackGain;
		state.Audio.Headphones.SidetoneGain = SidetoneGain;
	}
}
