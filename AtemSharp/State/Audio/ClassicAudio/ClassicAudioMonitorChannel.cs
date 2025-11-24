namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Classic audio monitor channel properties
/// </summary>
public class ClassicAudioMonitorChannel
{
	/// <summary>
	/// Whether the monitor is enabled
	/// </summary>
	public bool Enabled { get; internal set; }

	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain { get; internal set; }

	/// <summary>
	/// Whether the monitor is muted
	/// </summary>
	public bool Mute { get; internal set; }

	/// <summary>
	/// Whether solo is enabled
	/// </summary>
	public bool Solo { get; internal set; }

	/// <summary>
	/// Solo source identifier
	/// </summary>
	public ushort SoloSource { get; internal set; }

	/// <summary>
	/// Whether dim is enabled
	/// </summary>
	public bool Dim { get; internal set; }

	/// <summary>
	/// Dim level as percentage (0.0 to 1.0)
	/// </summary>
	public double DimLevel { get; internal set; }
}
