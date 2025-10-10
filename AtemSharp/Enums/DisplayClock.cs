namespace AtemSharp.Enums;

/// <summary>
/// Display clock state (stopped, running, or reset)
/// </summary>
public enum DisplayClockClockState : byte
{
    /// <summary>
    /// Clock is stopped
    /// </summary>
    Stopped = 0,

    /// <summary>
    /// Clock is running
    /// </summary>
    Running = 1,

    /// <summary>
    /// Clock is reset
    /// </summary>
    Reset = 2
}

/// <summary>
/// Display clock mode (countdown, countup, or time of day)
/// </summary>
public enum DisplayClockClockMode : byte
{
    /// <summary>
    /// Countdown mode
    /// </summary>
    Countdown = 0,

    /// <summary>
    /// Count up mode
    /// </summary>
    Countup = 1,

    /// <summary>
    /// Time of day mode
    /// </summary>
    TimeOfDay = 2
}