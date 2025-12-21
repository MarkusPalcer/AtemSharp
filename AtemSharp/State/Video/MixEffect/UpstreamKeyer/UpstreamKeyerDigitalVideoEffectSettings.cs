using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AtemSharp.Types;
using AtemSharp.Types.Border;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// DVE (Digital Video Effects) settings for upstream keyer
/// </summary>
public class UpstreamKeyerDigitalVideoEffectSettings
{
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public SizeF Size { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public PointF Location { get; internal set; }

    public RectangleF Bounds => new(Location.X, Location.Y, Size.Width, Size.Height);

    /// <summary>
    /// Rotation angle in degrees
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public double Rotation { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public Shadow Shadow { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public Border Border { get; } = new();

    /// <summary>
    /// Mask configuration
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MaskProperties Mask { get; } = new();

    /// <summary>
    /// Transition rate (frames)
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public byte Rate { get; internal set; }
}
