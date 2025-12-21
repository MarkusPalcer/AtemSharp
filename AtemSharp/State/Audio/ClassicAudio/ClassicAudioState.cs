using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Audio.ClassicAudio;

/// <summary>
/// Audio state for classic ATEM devices
/// </summary>
public class ClassicAudioState : AudioState
{
    internal ClassicAudioState()
    {
        Channels = new(id => new ClassicAudioChannel { Id = id });
    }

    /// <summary>
    /// Audio channels indexed by channel number
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, ClassicAudioChannel> Channels { get; }

    /// <summary>
    /// Master audio channel
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ClassicAudioMasterChannel Master { get; } = new();

    /// <summary>
    /// Headphones audio channel
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ClassicAudioHeadphoneOutputChannel Headphones { get; } = new();

    /// <summary>
    /// Monitor audio channel
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ClassicAudioMonitorChannel Monitor { get; } = new();

    /// <summary>
    /// Whether audio follows video crossfade transition
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public bool AudioFollowsVideo { get; internal set; }
}
