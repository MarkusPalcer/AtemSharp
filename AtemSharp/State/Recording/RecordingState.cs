namespace AtemSharp.State.Recording;

public class RecordingState
{
    public string FileName { get; internal set; } = string.Empty;
    public uint WorkingSet1DiskId { get; internal set; }
    public uint WorkingSet2DiskId { get; internal set; }
    public bool RecordInAllCameras { get; internal set; }
    public RecordingStatus Status { get; internal set; }
    public RecordingError Error { get; internal set; }
    public uint? RecordingTimeAvailable { get; internal set; }
    public RecordingDuration Duration { get; } = new();

    public Dictionary<uint, RecordingDisk> Disks { get; } = new();
}

public class RecordingDisk
{
    public uint DiskId { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public RecordingDiskStatus Status { get; internal set; }
    public uint RecordingTimeAvailable { get; internal set; }
}
