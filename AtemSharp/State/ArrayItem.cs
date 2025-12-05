using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public abstract class ArrayItem
{
    internal abstract void SetId(int id);
}
