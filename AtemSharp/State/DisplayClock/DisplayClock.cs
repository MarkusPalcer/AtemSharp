using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.DisplayClock;

/// <summary>
/// Display clock state container
/// </summary>
/// <remarks>
/// Other than in the TypeScript original, the properties are directly on this class,
/// not in a nested Properties object
/// </remarks>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class DisplayClock
{
    /// <summary>
    /// Whether the display clock is enabled
    /// </summary>
    public bool Enabled { get; internal set; }

    /// <summary>
    /// Size of the clock display
    /// </summary>
    public byte Size { get; internal set; }

    /// <summary>
    /// Opacity of the clock display (0-255)
    /// </summary>
    public byte Opacity { get; internal set; }

    /// <summary>
    /// X position of the clock display
    /// </summary>
    public double PositionX { get; internal set; }

    /// <summary>
    /// Y position of the clock display
    /// </summary>
    public double PositionY { get; internal set; }

    /// <summary>
    /// Whether the clock should auto-hide
    /// </summary>
    public bool AutoHide { get; internal set; }

    /// <summary>
    /// Starting time for countdown/countup modes
    /// </summary>
    public DisplayClockTime StartFrom { get; internal set; } = new();

    /// <summary>
    /// Clock mode (countdown, countup, time of day)
    /// </summary>
    public DisplayClockClockMode ClockMode { get; internal set; }

    /// <summary>
    /// Clock state (stopped, running, reset)
    /// </summary>
    public DisplayClockClockState ClockState { get; internal set; }

    /// <summary>
    /// Current time (only updated following a call to DisplayClockRequestTime)
    /// </summary>
    public DisplayClockTime CurrentTime { get; } = new();
}
