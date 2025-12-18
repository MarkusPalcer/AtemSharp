using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class AuxiliaryOutput
{
    public byte Id { get; internal init; }

    public ushort Source { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id}";
}
