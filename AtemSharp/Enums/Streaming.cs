namespace AtemSharp.Enums;

/// <summary>
/// Streaming errors
/// </summary>
[Flags]
public enum StreamingError
{
    None = 0,
    InvalidState = 1 << 4,
    Unknown = 1 << 15,
}

/// <summary>
/// Streaming status
/// </summary>
[Flags]
public enum StreamingStatus
{
    Idle = 1 << 0,
    Connecting = 1 << 1,
    Streaming = 1 << 2,
    Stopping = 1 << 5, // + Streaming
}

/// <summary>
/// Recording errors
/// </summary>
[Flags]
public enum RecordingError
{
    None = 1 << 1,
    NoMedia = 0,
    MediaFull = 1 << 2,
    MediaError = 1 << 3,
    MediaUnformatted = 1 << 4,
    DroppingFrames = 1 << 5,
    Unknown = 1 << 15,
}

/// <summary>
/// Recording status
/// </summary>
[Flags]
public enum RecordingStatus
{
    Idle = 0,
    Recording = 1 << 0,
    Stopping = 1 << 7,
}

/// <summary>
/// Recording disk status
/// </summary>
[Flags]
public enum RecordingDiskStatus
{
    Idle = 1 << 0,
    Unformatted = 1 << 1,
    Active = 1 << 2,
    Recording = 1 << 3,
    Removed = 1 << 5,
}