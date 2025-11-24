namespace AtemSharp.State;

/// <summary>
/// Downstream keyer mask configuration
/// </summary>
public class DownstreamKeyerMask
{
	/// <summary>
	/// Whether the mask is enabled
	/// </summary>
	public bool Enabled { get; internal set; }

	/// <summary>
	/// Top edge of the mask
	/// </summary>
	public double Top { get; internal set; }

	/// <summary>
	/// Bottom edge of the mask
	/// </summary>
	public double Bottom { get; internal set; }

	/// <summary>
	/// Left edge of the mask
	/// </summary>
	public double Left { get; internal set; }

	/// <summary>
	/// Right edge of the mask
	/// </summary>
	public double Right { get; internal set; }
}
