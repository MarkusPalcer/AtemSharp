namespace AtemSharp.State;

/// <summary>
/// Information about the Fairlight audio mixer capabilities
/// </summary>
public class FairlightAudioMixerInfo
{
    /// <summary>
    /// Number of Fairlight audio inputs available
    /// </summary>
    public byte Inputs { get; set; }

    /// <summary>
    /// Number of Fairlight monitor channels available
    /// </summary>
    public byte Monitors { get; set; }
}