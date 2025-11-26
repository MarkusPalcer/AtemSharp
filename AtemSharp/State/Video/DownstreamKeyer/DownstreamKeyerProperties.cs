namespace AtemSharp.State.Video.DownstreamKeyer;

/// <summary>
/// Complete downstream keyer properties including mask
/// </summary>
public class DownstreamKeyerProperties
{
    /// <summary>
    /// Whether to pre-multiply the key
    /// </summary>
    public bool PreMultiply { get; internal set; }

    /// <summary>
    /// Clip threshold (0.0 to 1.0)
    /// </summary>
    public double Clip { get; internal set; }

    /// <summary>
    /// Gain value (0.0 to 1.0)
    /// </summary>
    public double Gain { get; internal set; }

    /// <summary>
    /// Whether to invert the key
    /// </summary>
    public bool Invert { get; internal set; }

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
	public MaskProperties Mask { get; } = new();
}
