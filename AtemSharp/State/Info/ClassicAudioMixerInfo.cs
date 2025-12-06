using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Information about the audio mixer capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ClassicAudioMixerInfo : MixerInfo
{
    /// <summary>
    /// Number of audio inputs available
    /// </summary>
    public byte Inputs { get; internal set; }

    /// <summary>
    /// Number of monitor channels available
    /// </summary>
    public byte Monitors { get; internal set; }

    /// <summary>
    /// Number of headphone channels available
    /// </summary>
    public byte Headphones { get; internal set; }
}
