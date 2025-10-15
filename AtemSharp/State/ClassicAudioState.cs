namespace AtemSharp.State;

public abstract class AudioState;

/// <summary>
/// Audio state for classic ATEM devices
/// </summary>
public class ClassicAudioState : AudioState
{
    /// <summary>
    /// Audio channels indexed by channel number
    /// </summary>
    public Dictionary<int, ClassicAudioChannel> Channels { get; set; } = new();

    /// <summary>
    /// Master audio channel
    /// </summary>
    public ClassicAudioMasterChannel? Master { get; set; }

    /// <summary>
    /// Headphones audio channel
    /// </summary>
    public ClassicAudioHeadphoneOutputChannel? Headphones { get; set; } = new();

    /// <summary>
    /// Monitor audio channel
    /// </summary>
    public ClassicAudioMonitorChannel? Monitor { get; set; }

    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    public bool? AudioFollowsVideo { get; set; }

}
