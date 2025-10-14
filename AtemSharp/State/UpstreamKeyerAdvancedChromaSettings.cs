namespace AtemSharp.State;

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