using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Recording;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class RecordingState
{
    public string FileName { get; internal set; } = string.Empty;
    public uint WorkingSet1DiskId { get; internal set; }
    public uint WorkingSet2DiskId { get; internal set; }
    public bool RecordInAllCameras { get; internal set; }
    public RecordingStatus Status { get; internal set; }
    public RecordingError Error { get; internal set; }
    public uint? RecordingTimeAvailable { get; internal set; }
    public TimeCode Duration { get; } = new();

    public Dictionary<uint, RecordingDisk> Disks { get; } = new();
    public bool RecordAllInputs { get; internal set; }
}
