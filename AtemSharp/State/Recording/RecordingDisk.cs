using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Recording;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class RecordingDisk
{
    public uint DiskId { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public RecordingDiskStatus Status { get; internal set; }
    public uint RecordingTimeAvailable { get; internal set; }
}
