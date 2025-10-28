namespace AtemSharp.State;

public class RecordingState
{
    public string FileName { get; internal set; } = string.Empty;
    public uint WorkingSet1DiskId { get; internal set; }
    public uint WorkingSet2DiskId { get; internal set; }
    public bool RecordInAllCameras { get; internal set; }
    public RecordingStatus Status { get; internal set; }
    public RecordingError Error { get; internal set; }
    public uint? RecordingTimeAvailable { get; internal set; }
}
