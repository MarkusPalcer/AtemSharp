using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.Audio.ClassicAudio;

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
	public bool Enabled { get; init; }

	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain { get; init; }

	/// <summary>
	/// Whether the monitor is muted
	/// </summary>
	public bool Mute { get; init; }

	/// <summary>
	/// Whether solo is enabled
	/// </summary>
	public bool Solo { get; init; }

	/// <summary>
	/// Solo source identifier
	/// </summary>
	public int SoloSource { get; init; }

	/// <summary>
	/// Whether dim is enabled
	/// </summary>
	public bool Dim { get; init; }

	/// <summary>
	/// Dim level as percentage (0.0 to 1.0)
	/// </summary>
	public double DimLevel { get; init; }

	public static AudioMixerMonitorUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
	{
		return new AudioMixerMonitorUpdateCommand
		{
			Enabled = rawCommand.ReadBoolean(0),
			Gain = rawCommand.ReadUInt16BigEndian(2).UInt16ToDecibel(),
			Mute = rawCommand.ReadBoolean(4),
			Solo = rawCommand.ReadBoolean(5),
			SoloSource = rawCommand.ReadUInt16BigEndian(6),
			Dim = rawCommand.ReadBoolean(8),
			DimLevel = rawCommand.ReadUInt16BigEndian(10) / 100.0
		};
	}

	/// <inheritdoc />
	public void ApplyToState(AtemState state)
	{
        var audio = state.GetClassicAudio();
		audio.Monitor ??= new ClassicAudioMonitorChannel();
		audio.Monitor.Enabled = Enabled;
		audio.Monitor.Gain = Gain;
		audio.Monitor.Mute = Mute;
		audio.Monitor.Solo = Solo;
		audio.Monitor.SoloSource = SoloSource;
		audio.Monitor.Dim = Dim;
		audio.Monitor.DimLevel = DimLevel;
	}
}
