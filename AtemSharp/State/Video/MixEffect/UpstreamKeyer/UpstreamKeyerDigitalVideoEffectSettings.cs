using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AtemSharp.Types.Border;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// DVE (Digital Video Effects) settings for upstream keyer
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
public class UpstreamKeyerDigitalVideoEffectSettings
{
    public SizeF Size { get; internal set; }

    public PointF Location { get; internal set; }

    public RectangleF Bounds => new(Location.X, Location.Y, Size.Width, Size.Height);

    /// <summary>
    /// Rotation angle in degrees
    /// </summary>
    public double Rotation { get; internal set; }

    /// <summary>
    /// Whether shadow effect is enabled
    /// </summary>
    public bool ShadowEnabled { get; internal set; }


    public ExtendedBorderProperties Border { get; } = new();

    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool MaskEnabled { get; internal set; }

    /// <summary>
    /// Top edge of mask
    /// </summary>
    public double MaskTop { get; internal set; }

    /// <summary>
    /// Bottom edge of mask
    /// </summary>
    public double MaskBottom { get; internal set; }

    /// <summary>
    /// Left edge of mask
    /// </summary>
    public double MaskLeft { get; internal set; }

    /// <summary>
    /// Right edge of mask
    /// </summary>
    public double MaskRight { get; internal set; }

    /// <summary>
    /// Transition rate (frames)
    /// </summary>
    public byte Rate { get; internal set; }
}
