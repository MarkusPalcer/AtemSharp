namespace AtemSharp.State;

/// <summary>
/// Information about the audio mixer capabilities
/// </summary>
public class AudioMixerInfo
{
    /// <summary>
    /// Number of audio inputs available
    /// </summary>
    public byte Inputs { get; set; }

    /// <summary>
    /// Number of monitor channels available
    /// </summary>
    public byte Monitors { get; set; }

    /// <summary>
    /// Number of headphone channels available
    /// </summary>
    public byte Headphones { get; set; }
}