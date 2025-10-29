namespace AtemSharp.State.Streaming;

public enum StreamingStatus : ushort {
    Idle = 1 << 0,
    Connecting = 1 << 1,
    Streaming = 1 << 2,
    Stopping = 1 << 5, // + Streaming
}
