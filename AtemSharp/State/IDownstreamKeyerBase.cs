namespace AtemSharp.State;

/// <summary>
/// Base interface for downstream keyer state
/// </summary>
public interface IDownstreamKeyerBase
{
	/// <summary>
	/// Whether the downstream keyer is currently in transition
	/// </summary>
	bool InTransition { get; }

	/// <summary>
	/// Number of frames remaining in the current transition
	/// </summary>
	int RemainingFrames { get; }

	/// <summary>
	/// Whether the downstream keyer is in auto transition mode
	/// </summary>
	bool IsAuto { get; }

	/// <summary>
	/// Whether the downstream keyer is currently on air
	/// </summary>
	bool OnAir { get; set; }

	/// <summary>
	/// Direction of the auto transition (true = towards on air, false = towards off air)
	/// </summary>
	bool? IsTowardsOnAir { get; set; }
}