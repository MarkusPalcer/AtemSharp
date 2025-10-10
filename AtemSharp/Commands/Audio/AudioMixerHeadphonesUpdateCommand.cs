using System.Text;
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
	public double Gain { get; set; }
    
	/// <summary>
	/// Program out gain in decibel, -Infinity to +6dB
	/// </summary>
	public double ProgramOutGain { get; set; }
    
	/// <summary>
	/// Sidetone gain in decibel, -Infinity to +6dB
	/// </summary>
	public double SidetoneGain { get; set; }
    
	/// <summary>
	/// Talkback gain in decibel, -Infinity to +6dB
	/// </summary>
	public double TalkbackGain { get; set; }
	
	public static AudioMixerHeadphonesUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
	{
		using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true) ;
		
		return new AudioMixerHeadphonesUpdateCommand
		{
			Gain = reader.ReadUInt16BigEndian().UInt16ToDecibel(),
			ProgramOutGain = reader.ReadUInt16BigEndian().UInt16ToDecibel(),
			TalkbackGain = reader.ReadUInt16BigEndian().UInt16ToDecibel(),
			SidetoneGain = reader.ReadUInt16BigEndian().UInt16ToDecibel()
		};
	}

	/// <inheritdoc />
	public string[] ApplyToState(AtemState state)
	{
		if (state.Audio == null)
		{
			throw new InvalidIdError("Classic Audio", "headphones");
		}

		state.Audio.Headphones ??= new ClassicAudioHeadphoneOutputChannel();

		state.Audio.Headphones.Gain = Gain;
		state.Audio.Headphones.ProgramOutGain = ProgramOutGain;
		state.Audio.Headphones.TalkbackGain = TalkbackGain;
		state.Audio.Headphones.SidetoneGain = SidetoneGain;

		return ["audio.headphones"];
	}
}