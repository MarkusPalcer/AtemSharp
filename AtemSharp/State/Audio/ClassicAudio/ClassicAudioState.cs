namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Audio state for classic ATEM devices
/// </summary>
public class ClassicAudioState : AudioState
{

    // TODO: Is this an array? (My Device has Fairlight Audio)
    //       If so: Create array when applying AudioMixerConfigCommand (analogous to TopologyCommand)
    /// <summary>
    /// Audio channels indexed by channel number
    /// </summary>
    public Dictionary<int, ClassicAudioChannel> Channels { get; } = new();

    /// <summary>
    /// Master audio channel
    /// </summary>
    public ClassicAudioMasterChannel Master { get; } = new();

    /// <summary>
    /// Headphones audio channel
    /// </summary>
    public ClassicAudioHeadphoneOutputChannel Headphones { get; } = new();

    /// <summary>
    /// Monitor audio channel
    /// </summary>
    public ClassicAudioMonitorChannel Monitor { get; } = new();

    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    public bool AudioFollowsVideo { get; internal set; }
}
