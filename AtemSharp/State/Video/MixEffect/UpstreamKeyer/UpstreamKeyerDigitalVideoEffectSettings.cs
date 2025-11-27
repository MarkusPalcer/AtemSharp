using System.Diagnostics.CodeAnalysis;
using AtemSharp.State.Border;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// DVE (Digital Video Effects) settings for upstream keyer
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class UpstreamKeyerDigitalVideoEffectSettings
{
    /// <summary>
    /// Horizontal size scale factor
    /// </summary>
    public double SizeX { get; internal set; }

    /// <summary>
    /// Vertical size scale factor
    /// </summary>
    public double SizeY { get; internal set; }

    /// <summary>
    /// Horizontal position offset
    /// </summary>
    public double PositionX { get; internal set; }

    /// <summary>
    /// Vertical position offset
    /// </summary>
    public double PositionY { get; internal set; }

    /// <summary>
    /// Rotation angle in degrees
    /// </summary>
    public double Rotation { get; internal set; }

    /// <summary>
    /// Whether border effect is enabled
    /// </summary>
    public bool BorderEnabled { get; internal set; }

    /// <summary>
    /// Whether shadow effect is enabled
    /// </summary>
    public bool ShadowEnabled { get; internal set; }

    /// <summary>
    /// Type of border bevel effect
    /// </summary>
    public BorderBevel BorderBevel { get; internal set; }

    /// <summary>
    /// Outer border width
    /// </summary>
    public double BorderOuterWidth { get; internal set; }

    /// <summary>
    /// Inner border width
    /// </summary>
    public double BorderInnerWidth { get; internal set; }

    /// <summary>
    /// Outer border softness
    /// </summary>
    public double BorderOuterSoftness { get; internal set; }

    /// <summary>
    /// Inner border softness
    /// </summary>
    public double BorderInnerSoftness { get; internal set; }

    /// <summary>
    /// Border bevel softness
    /// </summary>
    public double BorderBevelSoftness { get; internal set; }

    /// <summary>
    /// Border bevel position
    /// </summary>
    public double BorderBevelPosition { get; internal set; }

    /// <summary>
    /// Border opacity (0-100)
    /// </summary>
    public double BorderOpacity { get; internal set; }

    /// <summary>
    /// Border color hue (0-360 degrees)
    /// </summary>
    public double BorderHue { get; internal set; }

    /// <summary>
    /// Border color saturation (0-100)
    /// </summary>
    public double BorderSaturation { get; internal set; }

    /// <summary>
    /// Border color luminance (0-100)
    /// </summary>
    public double BorderLuma { get; internal set; }

    /// <summary>
    /// Light source direction angle (0-360 degrees)
    /// </summary>
    public double LightSourceDirection { get; internal set; }

    /// <summary>
    /// Light source altitude (0-100)
    /// </summary>
    public double LightSourceAltitude { get; internal set; }

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
