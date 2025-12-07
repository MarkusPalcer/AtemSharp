using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Macro;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class Macro : ItemWithId<int>
{
    internal override void SetId(int id) => Id = (ushort)id;

    public ushort Id { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
    public string Description { get; internal set; } = string.Empty;
    public bool IsUsed { get; internal set; }
    public bool HasUnsupportedOps { get; internal set; }
}
