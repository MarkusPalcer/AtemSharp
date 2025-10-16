namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Classic audio master channel properties
/// </summary>
public class ClassicAudioMasterChannel
{
	/// <summary>
	/// Gain in decibel, -Infinity to +6dB
	/// </summary>
	public double Gain { get; set; }
    
	/// <summary>
	/// Balance, -50 to +50
	/// </summary>
	public double Balance { get; set; }
    
	/// <summary>
	/// Follow fade to black
	/// </summary>
	public bool FollowFadeToBlack { get; set; }
}