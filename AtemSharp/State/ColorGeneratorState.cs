using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ColorGeneratorState : ItemWithId<int>
{
    /// <summary>
    /// Gets the ID of the color generator
    /// </summary>
    public byte Id { get; internal set; }

    /// <summary>
    /// Gets the generated color
    /// </summary>
    public HslColor Color { get; internal set; }

    internal override void SetId(int id) => Id = (byte)id;
}
