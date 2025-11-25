namespace AtemSharp.State.Video;

public class MaskProperties
{
    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool Enabled { get; internal set; }

    /// <summary>
    /// Top edge of mask in units
    /// </summary>
    public double Top { get; internal set; }

    /// <summary>
    /// Bottom edge of mask in units
    /// </summary>
    public double Bottom { get; internal set; }

    /// <summary>
    /// Left edge of mask in units
    /// </summary>
    public double Left { get; internal set; }

    /// <summary>
    /// Right edge of mask in units
    /// </summary>
    public double Right { get; internal set; }
}
