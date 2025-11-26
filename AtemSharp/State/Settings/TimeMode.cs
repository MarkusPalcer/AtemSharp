namespace AtemSharp.State.Settings;

/// <summary>
/// Time mode for the ATEM device
/// </summary>
public enum TimeMode : byte
{
    /// <summary>
    /// Free running time mode
    /// </summary>
    FreeRun = 0,

    /// <summary>
    /// Time of day mode
    /// </summary>
    TimeOfDay = 1
}
