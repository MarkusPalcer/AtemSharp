namespace AtemSharp.Enums;

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