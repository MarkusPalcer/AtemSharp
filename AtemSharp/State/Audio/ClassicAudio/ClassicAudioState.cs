namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Audio state for classic ATEM devices
/// </summary>
public class ClassicAudioState : AudioState
{
    /// <summary>
    /// Audio channels indexed by channel number
    /// </summary>
    public Dictionary<int, ClassicAudioChannel> Channels { get; internal set; } = new();

    /// <summary>
    /// Master audio channel
    /// </summary>
    public ClassicAudioMasterChannel? Master { get; internal set; }

    /// <summary>
    /// Headphones audio channel
    /// </summary>
    public ClassicAudioHeadphoneOutputChannel? Headphones { get; internal set; } = new();

    /// <summary>
    /// Monitor audio channel
    /// </summary>
    public ClassicAudioMonitorChannel? Monitor { get; internal set; }

    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    public bool? AudioFollowsVideo { get; internal set; }

}
