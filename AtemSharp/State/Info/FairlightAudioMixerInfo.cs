using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Information about the Fairlight audio mixer capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class FairlightAudioMixerInfo : MixerInfo
{
    /// <summary>
    /// Number of Fairlight audio inputs available
    /// </summary>
    public byte Inputs { get; internal set; }

    /// <summary>
    /// Number of Fairlight monitor channels available
    /// </summary>
    public byte Monitors { get; internal set; }
}
