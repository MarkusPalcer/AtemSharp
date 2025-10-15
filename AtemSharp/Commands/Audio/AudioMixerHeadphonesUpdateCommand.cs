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
		if (state.Audio is not ClassicAudioState audio)
        {
            throw new InvalidOperationException("Cannot apply AudioMixerHeadphonesUpdateCommand to non-classic audio state");
        }

		audio.Headphones ??= new ClassicAudioHeadphoneOutputChannel();
		audio.Headphones.Gain = Gain;
		audio.Headphones.ProgramOutGain = ProgramOutGain;
		audio.Headphones.TalkbackGain = TalkbackGain;
		audio.Headphones.SidetoneGain = SidetoneGain;
	}
}
