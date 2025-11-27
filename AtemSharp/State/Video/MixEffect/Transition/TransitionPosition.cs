using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video.MixEffect.Transition;

/// <summary>
/// Current transition position information
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class TransitionPosition
{
	/// <summary>
	/// Whether a transition is currently in progress
	/// </summary>
	public bool InTransition { get; internal set; }

	/// <summary>
	/// Number of frames remaining in the transition
	/// </summary>
	public int RemainingFrames { get; internal set; }

	/// <summary>
	/// Current position of the transition handle (0.0 to 1.0)
	/// </summary>
	public double HandlePosition { get; internal set; }
}
