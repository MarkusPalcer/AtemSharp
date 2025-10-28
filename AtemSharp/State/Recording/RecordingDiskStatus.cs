namespace AtemSharp.State.Recording;

[Flags]
public enum RecordingDiskStatus : ushort {
    Idle = 1 << 0,
    Unformatted = 1 << 1,
    Active = 1 << 2,
    Recording = 1 << 3,
    Removed = 1 << 5,
    All = Idle | Unformatted | Active | Recording | Removed
}
