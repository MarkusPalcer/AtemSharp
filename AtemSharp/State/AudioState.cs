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
    /// Monitor audio channel
    /// </summary>
    public ClassicAudioMonitorChannel? Monitor { get; set; }
    
    /// <summary>
    /// Headphones audio channel
    /// </summary>
    public ClassicAudioHeadphoneOutputChannel? Headphones { get; set; }
}

/// <summary>
/// Classic audio channel properties
/// </summary>
public class ClassicAudioChannel
{
    /// <summary>
    /// Audio source type (readonly)
    /// </summary>
    public AudioSourceType SourceType { get; set; }
    
    /// <summary>
    /// External port type
    /// </summary>
    public ExternalPortType PortType { get; set; }
    
    /// <summary>
    /// Audio mix option
    /// </summary>
    public AudioMixOption MixOption { get; set; }
    
    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; set; }
    
    /// <summary>
    /// Balance, -50 to +50
    /// </summary>
    public double Balance { get; set; }
    
    /// <summary>
    /// Whether this channel supports RCA to XLR enabled setting (readonly)
    /// </summary>
    public bool SupportsRcaToXlrEnabled { get; set; }
    
    /// <summary>
    /// RCA to XLR enabled
    /// </summary>
    public bool RcaToXlrEnabled { get; set; }
}

/// <summary>
/// Classic audio master channel properties
/// </summary>
public class ClassicAudioMasterChannel
{
    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; set; }
    
    /// <summary>
    /// Balance, -50 to +50
    /// </summary>
    public double Balance { get; set; }
    
    /// <summary>
    /// Follow fade to black
    /// </summary>
    public bool FollowFadeToBlack { get; set; }
}

/// <summary>
/// Classic audio monitor channel properties
/// </summary>
public class ClassicAudioMonitorChannel
{
    /// <summary>
    /// Monitor enabled
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; set; }
    
    /// <summary>
    /// Mute enabled
    /// </summary>
    public bool Mute { get; set; }
    
    /// <summary>
    /// Solo enabled
    /// </summary>
    public bool Solo { get; set; }
    
    /// <summary>
    /// Solo input
    /// </summary>
    public int? SoloInput { get; set; }
    
    /// <summary>
    /// Dim enabled
    /// </summary>
    public bool Dim { get; set; }
}

/// <summary>
/// Classic audio headphone output channel properties
/// </summary>
public class ClassicAudioHeadphoneOutputChannel
{
    /// <summary>
    /// Gain in decibel, -Infinity to +6dB
    /// </summary>
    public double Gain { get; set; }
    
    /// <summary>
    /// Program out gain in decibel, -Infinity to +6dB
    /// </summary>
    public double ProgramOutGain { get; set; }
    
    /// <summary>
    /// Sidetone gain in decibel, -Infinity to +6dB
    /// </summary>
    public double SidetoneGain { get; set; }
    
    /// <summary>
    /// Talkback gain in decibel, -Infinity to +6dB
    /// </summary>
    public double TalkbackGain { get; set; }
}