namespace AtemSharp.State;

[Flags]
public enum RecordingStatus : ushort {
    Idle = 0,
    Recording = 1 << 0,
    Stopping = 1 << 7,
}
