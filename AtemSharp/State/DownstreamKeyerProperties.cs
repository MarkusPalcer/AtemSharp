namespace AtemSharp.State;

/// <summary>
/// Complete downstream keyer properties including mask
/// </summary>
public class DownstreamKeyerProperties : DownstreamKeyerGeneral
{
	/// <summary>
	/// Whether the keyer is tied to the program output
	/// </summary>
	public bool Tie { get; internal set; }

	/// <summary>
	/// Transition rate in frames
	/// </summary>
	public byte Rate { get; internal set; }

	/// <summary>
	/// Mask configuration
	/// </summary>
	public DownstreamKeyerMask Mask { get; internal set; } = new();
}
