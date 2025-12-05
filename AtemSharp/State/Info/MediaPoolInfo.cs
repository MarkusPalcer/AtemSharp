using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Media pool configuration and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MediaPoolInfo
{
    /// <summary>
    /// Number of still images available in the media pool
    /// </summary>
    public byte StillCount { get; internal set; }

    /// <summary>
    /// Number of video clips available in the media pool
    /// </summary>
    public byte ClipCount { get; internal set; }
}
