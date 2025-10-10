using AtemSharp.Enums;

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

/// <summary>
/// Display clock properties configuration
/// </summary>
public class DisplayClockProperties
{
    /// <summary>
    /// Whether the display clock is enabled
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Size of the clock display
    /// </summary>
    public byte Size { get; set; }

    /// <summary>
    /// Opacity of the clock display (0-255)
    /// </summary>
    public byte Opacity { get; set; }

    /// <summary>
    /// X position of the clock display
    /// </summary>
    public ushort PositionX { get; set; }

    /// <summary>
    /// Y position of the clock display
    /// </summary>
    public ushort PositionY { get; set; }

    /// <summary>
    /// Whether the clock should auto-hide
    /// </summary>
    public bool AutoHide { get; set; }

    /// <summary>
    /// Starting time for countdown/countup modes
    /// </summary>
    public DisplayClockTime StartFrom { get; set; } = new();

    /// <summary>
    /// Clock mode (countdown, countup, time of day)
    /// </summary>
    public DisplayClockClockMode ClockMode { get; set; }

    /// <summary>
    /// Clock state (stopped, running, reset)
    /// </summary>
    public DisplayClockClockState ClockState { get; set; }
}

/// <summary>
/// Display clock state container
/// </summary>
public class DisplayClockState
{
    /// <summary>
    /// Display clock properties configuration
    /// </summary>
    public DisplayClockProperties Properties { get; set; } = new();

    /// <summary>
    /// Current time (only updated following a call to DisplayClockRequestTime)
    /// </summary>
    public DisplayClockTime? CurrentTime { get; set; }
}