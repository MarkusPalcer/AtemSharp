using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public abstract class ItemWithId<TId>
{
    internal abstract void SetId(TId id);
}
