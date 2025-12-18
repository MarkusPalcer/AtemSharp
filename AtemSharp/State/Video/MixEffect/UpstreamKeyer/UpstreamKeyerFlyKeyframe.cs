using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AtemSharp.Types;
using AtemSharp.Types.Border;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

public class UpstreamKeyerFlyKeyframe
{
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public byte UpstreamKeyerId { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public byte MixEffectId { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public byte Id { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public SizeF Size { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public PointF Location { get; internal set; }

    public RectangleF Bounds => new(Location.X, Location.Y, Size.Width, Size.Height);

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public double Rotation { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public BorderProperties Border { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ShadowProperties Shadow { get; } = new();

    /// <summary>
    /// Mask configuration
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MaskProperties Mask { get; } = new();
}
