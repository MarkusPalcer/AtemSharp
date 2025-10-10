namespace AtemSharp.State;

/// <summary>
/// Complete downstream keyer properties including mask
/// </summary>
public class DownstreamKeyerProperties : DownstreamKeyerGeneral
{
	/// <summary>
	/// Whether the keyer is tied to the program output
	/// </summary>
	public bool Tie { get; set; }

	/// <summary>
	/// Transition rate in frames
	/// </summary>
	public int Rate { get; set; }

	/// <summary>
	/// Mask configuration
	/// </summary>
	public DownstreamKeyerMask Mask { get; set; } = new();
}