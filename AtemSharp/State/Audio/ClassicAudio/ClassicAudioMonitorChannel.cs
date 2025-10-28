namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Classic audio monitor channel properties
/// </summary>
public class ClassicAudioMonitorChannel
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
	public ushort SoloSource { get; set; }

	/// <summary>
	/// Whether dim is enabled
	/// </summary>
	public bool Dim { get; set; }

	/// <summary>
	/// Dim level as percentage (0.0 to 1.0)
	/// </summary>
	public double DimLevel { get; set; }
}
