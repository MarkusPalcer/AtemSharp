using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Types.Border;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ExtendedBorderProperties : BorderProperties
{
    public bool Enabled { get; internal set; }
    public BorderBevel Bevel { get; internal set; }
    public double LightSourceDirection { get;internal set; }
    public double LightSourceAltitude { get; internal set; }
}
