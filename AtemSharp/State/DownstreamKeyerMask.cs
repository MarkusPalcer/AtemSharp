namespace AtemSharp.State;

/// <summary>
/// Downstream keyer mask configuration
/// </summary>
public class DownstreamKeyerMask
{
	/// <summary>
	/// Whether the mask is enabled
	/// </summary>
	public bool Enabled { get; set; }

	/// <summary>
	/// Top edge of the mask
	/// </summary>
	public double Top { get; set; }

	/// <summary>
	/// Bottom edge of the mask
	/// </summary>
	public double Bottom { get; set; }

	/// <summary>
	/// Left edge of the mask
	/// </summary>
	public double Left { get; set; }

	/// <summary>
	/// Right edge of the mask
	/// </summary>
	public double Right { get; set; }
}