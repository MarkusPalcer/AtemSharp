using System.Text;
using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Audio;

/// <summary>
/// Update command for audio mixer monitor properties
/// </summary>
[Command("AMmO")]
public class AudioMixerMonitorUpdateCommand : IDeserializedCommand
{
	/// <summary>
	/// Whether the monitor is enabled
	/// </summary>
	public bool Enabled { get; set; }
	
	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain { get; set; }

	/// <summary>
	/// Whether the monitor is muted
	/// </summary>
	public bool Mute { get; set; }

	/// <summary>
	/// Whether solo is enabled
	/// </summary>
	public bool Solo { get; set; }
	
	/// <summary>
	/// Solo source identifier
	/// </summary>
	public int SoloSource { get; set; }

	/// <summary>
	/// Whether dim is enabled
	/// </summary>
	public bool Dim { get; set; }
	
	/// <summary>
	/// Dim level as percentage (0.0 to 1.0)
	/// </summary>
	public double DimLevel { get; set; }
	
	public static AudioMixerMonitorUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
	{
		using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
		
		var enabled = reader.ReadByte() > 0; // byte 0
		reader.ReadByte(); // Skip byte 1 (padding)
		var gain = reader.ReadUInt16BigEndian().UInt16ToDecibel(); // bytes 2-3
		var mute = reader.ReadByte() > 0; // byte 4
		var solo = reader.ReadByte() > 0; // byte 5
		var soloSource = reader.ReadUInt16BigEndian(); // bytes 6-7
		var dim = reader.ReadByte() > 0; // byte 8
		reader.ReadByte(); // Skip byte 9 (padding)
		var dimLevel = reader.ReadUInt16BigEndian() / 100.0; // bytes 10-11
		
		return new AudioMixerMonitorUpdateCommand
		{
			Enabled = enabled,
			Gain = gain,
			Mute = mute,
			Solo = solo,
			SoloSource = soloSource,
			Dim = dim,
			DimLevel = dimLevel
		};
	}

	/// <inheritdoc />
	public string[] ApplyToState(AtemState state)
	{
		if (state.Audio == null)
		{
			throw new InvalidIdError("Classic Audio", "monitor");
		}

		state.Audio.Monitor ??= new ClassicAudioMonitorChannel();

		state.Audio.Monitor.Enabled = Enabled;
		state.Audio.Monitor.Gain = Gain;
		state.Audio.Monitor.Mute = Mute;
		state.Audio.Monitor.Solo = Solo;
		state.Audio.Monitor.SoloSource = SoloSource;
		state.Audio.Monitor.Dim = Dim;
		state.Audio.Monitor.DimLevel = DimLevel;

		return ["audio.monitor"];
	}
}