using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// Advanced chroma key settings for upstream keyer
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class UpstreamKeyerAdvancedChromaSettings
{
    /// <summary>
    /// Advanced chroma key properties
    /// </summary>
    public UpstreamKeyerAdvancedChromaProperties Properties { get; } = new();

    /// <summary>
    /// Advanced chroma key sample settings
    /// </summary>
    public UpstreamKeyerAdvancedChromaSample Sample { get; } = new();
}
