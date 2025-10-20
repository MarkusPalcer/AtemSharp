namespace AtemSharp.State;

/// <summary>
/// Display clock time representation
/// </summary>
public class DisplayClockTime
{
	/// <summary>
	/// Hours (0-23)
	/// </summary>
	public byte Hours { get; set; }

	/// <summary>
	/// Minutes (0-59)
	/// </summary>
	public byte Minutes { get; set; }

	/// <summary>
	/// Seconds (0-59)
	/// </summary>
	public byte Seconds { get; set; }

	/// <summary>
	/// Frames (0-59, depends on frame rate)
	/// </summary>
	public byte Frames { get; set; }
}