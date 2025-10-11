namespace AtemSharp.State;

/// <summary>
/// Fade to black properties for a mix effect
/// </summary>
public class FadeToBlackProperties
{
	/// <summary>
	/// Whether the output is fully black
	/// </summary>
	public bool IsFullyBlack { get; set; }

	/// <summary>
	/// Whether a fade to black transition is in progress
	/// </summary>
	public bool InTransition { get; set; }

	/// <summary>
	/// Number of frames remaining in the transition
	/// </summary>
	public int RemainingFrames { get; set; }

	/// <summary>
	/// Fade to black rate (frames)
	/// </summary>
	public int Rate { get; set; }
}