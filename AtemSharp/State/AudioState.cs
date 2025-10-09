using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Audio state for classic ATEM devices
/// </summary>
public class AudioState
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
    
}