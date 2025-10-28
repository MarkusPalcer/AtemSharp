using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// DVE (Digital Video Effects) settings for upstream keyer
/// </summary>
public class UpstreamKeyerDVESettings
{
    /// <summary>
    /// Horizontal size scale factor
    /// </summary>
    public double SizeX { get; set; }

    /// <summary>
    /// Vertical size scale factor
    /// </summary>
    public double SizeY { get; set; }

    /// <summary>
    /// Horizontal position offset
    /// </summary>
    public double PositionX { get; set; }

    /// <summary>
    /// Vertical position offset
    /// </summary>
    public double PositionY { get; set; }

    /// <summary>
    /// Rotation angle in degrees
    /// </summary>
    public double Rotation { get; set; }

    /// <summary>
    /// Whether border effect is enabled
    /// </summary>
    public bool BorderEnabled { get; set; }

    /// <summary>
    /// Whether shadow effect is enabled
    /// </summary>
    public bool ShadowEnabled { get; set; }

    /// <summary>
    /// Type of border bevel effect
    /// </summary>
    public BorderBevel BorderBevel { get; set; }

    /// <summary>
    /// Outer border width
    /// </summary>
    public double BorderOuterWidth { get; set; }

    /// <summary>
    /// Inner border width
    /// </summary>
    public double BorderInnerWidth { get; set; }

    /// <summary>
    /// Outer border softness
    /// </summary>
    public double BorderOuterSoftness { get; set; }

    /// <summary>
    /// Inner border softness
    /// </summary>
    public double BorderInnerSoftness { get; set; }

    /// <summary>
    /// Border bevel softness
    /// </summary>
    public double BorderBevelSoftness { get; set; }

    /// <summary>
    /// Border bevel position
    /// </summary>
    public double BorderBevelPosition { get; set; }

    /// <summary>
    /// Border opacity (0-100)
    /// </summary>
    public double BorderOpacity { get; set; }

    /// <summary>
    /// Border color hue (0-360 degrees)
    /// </summary>
    public double BorderHue { get; set; }

    /// <summary>
    /// Border color saturation (0-100)
    /// </summary>
    public double BorderSaturation { get; set; }

    /// <summary>
    /// Border color luminance (0-100)
    /// </summary>
    public double BorderLuma { get; set; }

    /// <summary>
    /// Light source direction angle (0-360 degrees)
    /// </summary>
    public double LightSourceDirection { get; set; }

    /// <summary>
    /// Light source altitude (0-100)
    /// </summary>
    public double LightSourceAltitude { get; set; }

    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool MaskEnabled { get; set; }

    /// <summary>
    /// Top edge of mask
    /// </summary>
    public double MaskTop { get; set; }

    /// <summary>
    /// Bottom edge of mask
    /// </summary>
    public double MaskBottom { get; set; }

    /// <summary>
    /// Left edge of mask
    /// </summary>
    public double MaskLeft { get; set; }

    /// <summary>
    /// Right edge of mask
    /// </summary>
    public double MaskRight { get; set; }

    /// <summary>
    /// Transition rate (frames)
    /// </summary>
    public byte Rate { get; set; }
}