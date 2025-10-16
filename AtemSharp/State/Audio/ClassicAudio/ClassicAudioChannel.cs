using AtemSharp.Enums.Audio;
using AtemSharp.Enums.Ports;

namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Classic audio channel properties
/// </summary>
public class ClassicAudioChannel
{
	/// <summary>
	/// Audio source type (readonly)
	/// </summary>
	public AudioSourceType SourceType { get; set; }

	/// <summary>
	/// External port type
	/// </summary>
	public ExternalPortType PortType { get; set; }

	/// <summary>
	/// Audio mix option
	/// </summary>
	public AudioMixOption MixOption { get; set; }

	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain { get; set; }

	/// <summary>
	/// Balance, -50 to +50
	/// </summary>
	public double Balance { get; set; }

	/// <summary>
	/// Whether this channel supports RCA to XLR enabled setting (readonly)
	/// </summary>
	public bool SupportsRcaToXlrEnabled { get; set; }

	/// <summary>
	/// RCA to XLR enabled
	/// </summary>
	public bool RcaToXlrEnabled { get; set; }
}
