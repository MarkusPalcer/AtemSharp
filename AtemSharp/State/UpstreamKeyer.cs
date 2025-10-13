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