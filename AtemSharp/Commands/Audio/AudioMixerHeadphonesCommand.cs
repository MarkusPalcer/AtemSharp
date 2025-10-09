using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Command to update audio mixer headphones properties
/// </summary>
[Command("CAMH")]
public class AudioMixerHeadphonesCommand : SerializedCommand
{
	private double _gain;
	private double _programOutGain;
	private double _talkbackGain;
	private double _sidetoneGain;

	public AudioMixerHeadphonesCommand(AtemState currentState)
	{
		// If the audio state or headphones do not exist, initialize to default values
		// by setting the properties, thus setting the changed-flag for each property
		if (currentState.Audio?.Headphones is null)
		{
			Gain = 0.0;
			ProgramOutGain = 0.0;
			TalkbackGain = 0.0;
			SidetoneGain = 0.0;
			return;
		}

		_gain = currentState.Audio.Headphones.Gain;
		_programOutGain = currentState.Audio.Headphones.ProgramOutGain;
		_talkbackGain = currentState.Audio.Headphones.TalkbackGain;
		_sidetoneGain = currentState.Audio.Headphones.SidetoneGain;
	}

	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain
	{
		get => _gain;
		set
		{
			_gain = value;
			Flag |= 1 << 0;
		}
	}

	/// <summary>
	/// Program out gain in decibel, -Infinity to +6dB
	/// </summary>
	public double ProgramOutGain
	{
		get => _programOutGain;
		set
		{
			_programOutGain = value;
			Flag |= 1 << 1;
		}
	}

	/// <summary>
	/// Talkback gain in decibel, -Infinity to +6dB
	/// </summary>
	public double TalkbackGain
	{
		get => _talkbackGain;
		set
		{
			_talkbackGain = value;
			Flag |= 1 << 2;
		}
	}

	/// <summary>
	/// Sidetone gain in decibel, -Infinity to +6dB
	/// </summary>
	public double SidetoneGain
	{
		get => _sidetoneGain;
		set
		{
			_sidetoneGain = value;
			Flag |= 1 << 3;
		}
	}

	/// <inheritdoc />
	public override byte[] Serialize(ProtocolVersion version)
	{
		using var memoryStream = new MemoryStream(12);
		using var writer = new BinaryWriter(memoryStream);
		
		writer.Write((byte)Flag);
		writer.Pad(1);
		writer.WriteUInt16BigEndian(Gain.DecibelToUInt16());
		writer.WriteUInt16BigEndian(ProgramOutGain.DecibelToUInt16());
		writer.WriteUInt16BigEndian(TalkbackGain.DecibelToUInt16());
		writer.WriteUInt16BigEndian(SidetoneGain.DecibelToUInt16());
		writer.Pad(2);	

		return memoryStream.ToArray();
	}
}