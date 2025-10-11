namespace AtemSharp.State;

/// <summary>
/// Current transition position information
/// </summary>
public class TransitionPosition
{
	/// <summary>
	/// Whether a transition is currently in progress
	/// </summary>
	public bool InTransition { get; set; }

	/// <summary>
	/// Number of frames remaining in the transition
	/// </summary>
	public int RemainingFrames { get; set; }

	/// <summary>
	/// Current position of the transition handle (0.0 to 1.0)
	/// </summary>
	public double HandlePosition { get; set; }
}