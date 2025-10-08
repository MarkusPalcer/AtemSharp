namespace AtemSharp.Enums;

/// <summary>
/// Multi-viewer layouts
/// </summary>
[Flags]
public enum MultiViewerLayout
{
    Default = 0,
    TopLeftSmall = 1,
    TopRightSmall = 2,
    ProgramBottom = TopLeftSmall | TopRightSmall,
    BottomLeftSmall = 4,
    ProgramRight = TopLeftSmall | BottomLeftSmall,
    BottomRightSmall = 8,
    ProgramLeft = TopRightSmall | BottomRightSmall,
    ProgramTop = BottomLeftSmall | BottomRightSmall,
}

/// <summary>
/// Display clock state
/// </summary>
public enum DisplayClockClockState
{
    Stopped = 0,
    Running = 1,
    Reset = 2,
}

/// <summary>
/// Display clock mode
/// </summary>
public enum DisplayClockClockMode
{
    Countdown = 0,
    Countup = 1,
    TimeOfDay = 2,
}

/// <summary>
/// Time mode
/// </summary>
public enum TimeMode
{
    FreeRun = 0,
    TimeOfDay = 1,
}