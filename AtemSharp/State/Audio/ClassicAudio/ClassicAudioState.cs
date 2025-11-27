using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Audio state for classic ATEM devices
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class ClassicAudioState : AudioState
{
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
