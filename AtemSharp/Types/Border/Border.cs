using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Types.Border;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class Border : BorderProperties
{
    public bool Enabled { get; internal set; }
    public BorderBevel Bevel { get; internal set; }

}
