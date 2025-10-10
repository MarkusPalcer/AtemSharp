namespace AtemSharp.State;

/// <summary>
/// Main ATEM state container
/// </summary>
public class AtemState
{
    /// <summary>
    /// Device information and capabilities
    /// </summary>
    public DeviceInfo Info { get; set; } = new();

    /// <summary>
    /// Audio state for classic ATEM devices
    /// </summary>
    public AudioState? Audio { get; set; }

    /// <summary>
    /// Fairlight audio state for ATEM devices with Fairlight audio support
    /// </summary>
    public FairlightAudioState? Fairlight { get; set; }
}