using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ColorGeneratorState
{
    /// <summary>
    /// Gets the ID of the color generator
    /// </summary>
    public byte Id { get; internal init; }

    /// <summary>
    /// Gets the generated color
    /// </summary>
    public HslColor Color { get; internal set; }
}
