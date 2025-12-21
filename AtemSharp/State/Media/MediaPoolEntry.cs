using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Media;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class MediaPoolEntry
{
    public ushort Id { get; internal init; }
    public bool IsUsed { get; internal set; }

    public string Name { get; internal set; } = string.Empty;

    public string Hash { get; internal set; } = string.Empty;
    public string FileName { get; internal set; } = string.Empty;
    public ushort FrameCount { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id}";
}
