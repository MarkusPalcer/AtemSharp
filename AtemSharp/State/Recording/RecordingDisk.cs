using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Recording;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class RecordingDisk
{
    public uint DiskId { get; internal init; }
    public string Name { get; internal set; } = string.Empty;
    public RecordingDiskStatus Status { get; internal set; }
    public uint RecordingTimeAvailable { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{DiskId}";
}
