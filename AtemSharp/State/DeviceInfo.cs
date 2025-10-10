namespace AtemSharp.State;

/// <summary>
/// Device information and capabilities
/// </summary>
public class DeviceInfo
{
    /// <summary>
    /// Audio mixer configuration and capabilities
    /// </summary>
    public AudioMixerInfo? AudioMixer { get; set; }

    /// <summary>
    /// Fairlight audio mixer configuration and capabilities
    /// </summary>
    public FairlightAudioMixerInfo? FairlightMixer { get; set; }

    /// <summary>
    /// Macro pool configuration and capabilities
    /// </summary>
    public MacroPoolInfo? MacroPool { get; set; }
}