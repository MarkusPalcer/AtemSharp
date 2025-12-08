using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.Types.Border;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class BorderProperties
{
    public double OuterWidth { get; internal set; }
    public double InnerWidth { get; internal set; }
    public byte OuterSoftness { get; internal set; }
    public byte InnerSoftness { get; internal set; }
    public byte BevelSoftness { get; internal set; }
    public byte BevelPosition { get; internal set; }
    public byte Opacity { get; internal set; }
    public HslColor Color { get; internal set; }
}
