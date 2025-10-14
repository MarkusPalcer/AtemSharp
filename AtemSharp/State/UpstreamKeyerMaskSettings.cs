namespace AtemSharp.State;

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