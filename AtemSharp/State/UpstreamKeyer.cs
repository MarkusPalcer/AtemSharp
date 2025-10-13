using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Upstream keyer state containing key properties and settings
/// </summary>
public class UpstreamKeyer
{
    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Whether the keyer is currently on air
    /// </summary>
    public bool OnAir { get; set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource { get; set; }

    /// <summary>
    /// Cut source input number
    /// </summary>
    public int CutSource { get; set; }

    /// <summary>
    /// Type of keying effect (Luma, Chroma, Pattern, DVE)
    /// </summary>
    public MixEffectKeyType KeyType { get; set; }

    /// <summary>
    /// Whether this keyer supports fly key functionality
    /// </summary>
    public bool CanFlyKey { get; set; }

    /// <summary>
    /// Whether fly key is currently enabled
    /// </summary>
    public bool FlyEnabled { get; set; }

    /// <summary>
    /// Mask settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerMaskSettings? MaskSettings { get; set; }

    /// <summary>
    /// Luma key settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerLumaSettings? LumaSettings { get; set; }

    /// <summary>
    /// Advanced chroma key settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerAdvancedChromaSettings? AdvancedChromaSettings { get; set; }

    /// <summary>
    /// DVE (Digital Video Effects) settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerDVESettings? DVESettings { get; set; }
    
    /// <summary>
    /// Fly properties for the upstream keyer
    /// </summary>
    public UpstreamKeyerFlyProperties? FlyProperties { get; set; }
}

/// <summary>
/// Mask settings for upstream keyer
/// </summary>
public class UpstreamKeyerMaskSettings
{
    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool MaskEnabled { get; set; }

    /// <summary>
    /// Top edge of mask in units
    /// </summary>
    public double MaskTop { get; set; }

    /// <summary>
    /// Bottom edge of mask in units
    /// </summary>
    public double MaskBottom { get; set; }

    /// <summary>
    /// Left edge of mask in units
    /// </summary>
    public double MaskLeft { get; set; }

    /// <summary>
    /// Right edge of mask in units
    /// </summary>
    public double MaskRight { get; set; }
}

/// <summary>
/// Luma key settings for upstream keyer
/// </summary>
public class UpstreamKeyerLumaSettings
{
    /// <summary>
    /// Whether the key should be treated as premultiplied
    /// </summary>
    public bool PreMultiplied { get; set; }

    /// <summary>
    /// Clip threshold value
    /// </summary>
    public double Clip { get; set; }

    /// <summary>
    /// Gain value for the luma key
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    public bool Invert { get; set; }
}

/// <summary>
/// Advanced chroma key settings for upstream keyer
/// </summary>
public class UpstreamKeyerAdvancedChromaSettings
{
    /// <summary>
    /// Advanced chroma key properties
    /// </summary>
    public UpstreamKeyerAdvancedChromaProperties? Properties { get; set; }

    /// <summary>
    /// Advanced chroma key sample settings
    /// </summary>
    public UpstreamKeyerAdvancedChromaSample? Sample { get; set; }
}

/// <summary>
/// Advanced chroma key properties for upstream keyer
/// </summary>
public class UpstreamKeyerAdvancedChromaProperties
{
    /// <summary>
    /// Foreground level value
    /// </summary>
    public double ForegroundLevel { get; set; }

    /// <summary>
    /// Background level value
    /// </summary>
    public double BackgroundLevel { get; set; }

    /// <summary>
    /// Key edge value
    /// </summary>
    public double KeyEdge { get; set; }

    /// <summary>
    /// Spill suppression value
    /// </summary>
    public double SpillSuppression { get; set; }

    /// <summary>
    /// Flare suppression value
    /// </summary>
    public double FlareSuppression { get; set; }

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    public double Brightness { get; set; }

    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    public double Contrast { get; set; }

    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    public double Saturation { get; set; }

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    public double Red { get; set; }

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    public double Green { get; set; }

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    public double Blue { get; set; }
}

/// <summary>
/// Advanced chroma key sample settings for upstream keyer
/// </summary>
public class UpstreamKeyerAdvancedChromaSample
{
    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    public bool EnableCursor { get; set; }

    /// <summary>
    /// Whether to show preview
    /// </summary>
    public bool Preview { get; set; }

    /// <summary>
    /// Cursor X position
    /// </summary>
    public double CursorX { get; set; }

    /// <summary>
    /// Cursor Y position
    /// </summary>
    public double CursorY { get; set; }

    /// <summary>
    /// Cursor size
    /// </summary>
    public double CursorSize { get; set; }

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    public double SampledY { get; set; }

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    public double SampledCb { get; set; }

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    public double SampledCr { get; set; }
}

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
    public int Rate { get; set; }
}

/// <summary>
/// Fly properties for upstream keyer
/// </summary>
public class UpstreamKeyerFlyProperties
{
    /// <summary>
    /// Whether key frame A is set
    /// </summary>
    public bool IsASet { get; set; }
    
    /// <summary>
    /// Whether key frame B is set
    /// </summary>
    public bool IsBSet { get; set; }
    
    /// <summary>
    /// Current key frame state flags
    /// </summary>
    public IsAtKeyFrame IsAtKeyFrame { get; set; }
    
    /// <summary>
    /// Run to infinite index
    /// </summary>
    public int RunToInfiniteIndex { get; set; }
}