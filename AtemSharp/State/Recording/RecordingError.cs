namespace AtemSharp.State.Recording;

[Flags]
public enum RecordingError : ushort {
    NoMedia = 0,
    None = 1 << 1,
    MediaFull = 1 << 2,
    MediaError = 1 << 3,
    MediaUnformatted = 1 << 4,
    DroppingFrames = 1 << 5,
    Unknown = 1 << 15,
}
