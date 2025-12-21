using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Recording;

[ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
public class RecordingState
{
    internal RecordingState()
    {
        Disks = new ItemCollection<uint, RecordingDisk>(id => new RecordingDisk
        {
            DiskId = id
        });
    }

    public string FileName { get; internal set; } = string.Empty;
    public uint WorkingSet1DiskId { get; internal set; }
    public uint WorkingSet2DiskId { get; internal set; }
    public bool RecordInAllCameras { get; internal set; }
    public RecordingStatus Status { get; internal set; }
    public RecordingError Error { get; internal set; }
    public uint? RecordingTimeAvailable { get; internal set; }
    public TimeCode Duration { get; } = new();

    public ItemCollection<uint, RecordingDisk> Disks { get; }
    public bool RecordAllInputs { get; internal set; }
}
