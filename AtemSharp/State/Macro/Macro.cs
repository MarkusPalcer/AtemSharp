using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Macro;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class Macro
{
    public ushort Id { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;
    public bool IsUsed { get; internal set; }
    public bool HasUnsupportedOps { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id}";
}
