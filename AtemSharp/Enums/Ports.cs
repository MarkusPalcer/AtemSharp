namespace AtemSharp.Enums;

/// <summary>
/// Macro actions
/// </summary>
public enum MacroAction
{
    Run = 0,
    Stop = 1,
    StopRecord = 2,
    InsertUserWait = 3,
    Continue = 4,
    Delete = 5,
}

/// <summary>
/// External port types
/// </summary>
[Flags]
public enum ExternalPortType
{
    Unknown = 0,
    SDI = 1,
    HDMI = 2,
    Component = 4,
    Composite = 8,
    SVideo = 16,
    XLR = 32,
    AESEBU = 64,
    RCA = 128,
    Internal = 256,
    TSJack = 512,
    MADI = 1024,
    TRSJack = 2048,
    RJ45 = 4096,
}

/// <summary>
/// Internal port types
/// </summary>
public enum InternalPortType
{
    External = 0,
    Black = 1,
    ColorBars = 2,
    ColorGenerator = 3,
    MediaPlayerFill = 4,
    MediaPlayerKey = 5,
    SuperSource = 6,
    // Since V8_1_1
    ExternalDirect = 7,

    MEOutput = 128,
    Auxiliary = 129,
    Mask = 130,
    // Since V8_1_1
    MultiViewer = 131,
}

/// <summary>
/// Source availability flags
/// </summary>
[Flags]
public enum SourceAvailability
{
    None = 0,
    Auxiliary = 1 << 0,
    Multiviewer = 1 << 1,
    SuperSourceArt = 1 << 2,
    SuperSourceBox = 1 << 3,
    KeySource = 1 << 4,
    Auxiliary1 = 1 << 5,
    Auxiliary2 = 1 << 6,
    All = Auxiliary | Multiviewer | SuperSourceArt | SuperSourceBox | KeySource | Auxiliary1 | Auxiliary2,
}

/// <summary>
/// Mix Engine (ME) availability flags
/// </summary>
[Flags]
public enum MeAvailability
{
    None = 0,
    Me1 = 1 << 0,
    Me2 = 1 << 1,
    Me3 = 1 << 2,
    Me4 = 1 << 3,
    All = Me1 | Me2 | Me3 | Me4,
}