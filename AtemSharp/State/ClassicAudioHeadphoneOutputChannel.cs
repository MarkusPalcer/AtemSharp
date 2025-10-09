namespace AtemSharp.State;

/// <summary>
/// Classic audio headphone output channel properties
/// </summary>
public class ClassicAudioHeadphoneOutputChannel
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
}