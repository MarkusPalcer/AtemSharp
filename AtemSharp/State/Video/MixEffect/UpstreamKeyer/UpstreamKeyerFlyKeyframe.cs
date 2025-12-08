using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AtemSharp.Types;
using AtemSharp.Types.Border;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class UpstreamKeyerFlyKeyframe
{
    public byte UpstreamKeyerId { get; internal set; }

    public byte MixEffectId { get; internal set; }

    public byte Id { get; internal set; }
    public SizeF Size { get; internal set; }
    public PointF Location { get; internal set; }
    public RectangleF Bounds => new(Location.X, Location.Y, Size.Width, Size.Height);
    public double Rotation { get; internal set; }
    public BorderProperties Border { get; } = new();
    public double LightSourceDirection { get; internal set; }
    public byte LightSourceAltitude { get; internal set; }

    /// <summary>
    /// Mask configuration
    /// </summary>
    public MaskProperties Mask { get; } = new();
}
