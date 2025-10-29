namespace AtemSharp.State.Streaming;

public enum StreamingError : ushort {
    None,
    InvalidState = 1 << 4,
    Unknown = 1 << 15,
}
