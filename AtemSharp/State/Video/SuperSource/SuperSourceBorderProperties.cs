using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types.Border;

namespace AtemSharp.State.Video.SuperSource;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SuperSourceBorderProperties : BorderProperties
{
    public bool Enabled { get; internal set; }
    public BorderBevel Bevel { get; internal set; }
    public double LightSourceDirection { get;internal set; }
    public double LightSourceAltitude { get; internal set; }
}
