namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// Luma key settings for upstream keyer
/// </summary>
public class UpstreamKeyerLumaSettings
{
    /// <summary>
    /// Whether the key should be treated as premultiplied
    /// </summary>
    public bool PreMultiplied { get; internal set; }

    /// <summary>
    /// Clip threshold value
    /// </summary>
    public double Clip { get; internal set; }

    /// <summary>
    /// Gain value for the luma key
    /// </summary>
    public double Gain { get; internal set; }

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    public bool Invert { get; internal set; }
}
