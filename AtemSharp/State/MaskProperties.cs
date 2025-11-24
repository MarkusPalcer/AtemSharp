namespace AtemSharp.State;

/// <summary>
/// Mask settings for upstream keyer
/// </summary>
public class MaskProperties
{
    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Top edge of mask in units
    /// </summary>
    public double Top { get; set; }

    /// <summary>
    /// Bottom edge of mask in units
    /// </summary>
    public double Bottom { get; set; }

    /// <summary>
    /// Left edge of mask in units
    /// </summary>
    public double Left { get; set; }

    /// <summary>
    /// Right edge of mask in units
    /// </summary>
    public double Right { get; set; }
}
