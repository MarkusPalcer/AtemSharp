namespace AtemSharp.State;

/// <summary>
/// Downstream keyer general properties
/// </summary>
public class DownstreamKeyerGeneral
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
}
