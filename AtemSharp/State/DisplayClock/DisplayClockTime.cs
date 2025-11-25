namespace AtemSharp.State;

/// <summary>
/// Display clock time representation
/// </summary>
public class DisplayClockTime
{
    /// <summary>
    /// Hours (0-23)
    /// </summary>
    public byte Hours { get; internal set; }

    /// <summary>
    /// Minutes (0-59)
    /// </summary>
    public byte Minutes { get; internal set; }

    /// <summary>
    /// Seconds (0-59)
    /// </summary>
    public byte Seconds { get; internal set; }

    /// <summary>
    /// Frames (0-59, depends on frame rate)
    /// </summary>
    public byte Frames { get; internal set; }
}
