using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Device information and capabilities
/// </summary>
public class DeviceInfo
{
    /// <summary>
    /// Product identifier string from the device
    /// </summary>
    public string? ProductIdentifier { get; set; }

    /// <summary>
    /// ATEM device model
    /// </summary>
    public Model Model { get; set; }

    /// <summary>
    /// ATEM protocol API version
    /// </summary>
    public ProtocolVersion ApiVersion { get; set; } = ProtocolVersion.Unknown;

    /// <summary>
    /// Power supply status for each power supply in the device
    /// </summary>
    public bool[] Power { get; set; } = [];

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
    public MacroPoolInfo MacroPool { get; set; } = new();

    /// <summary>
    /// Media pool configuration and capabilities
    /// </summary>
    public MediaPoolInfo MediaPool { get; set; } = new();

    /// <summary>
    /// SuperSource configurations and capabilities
    /// </summary>
    public Dictionary<int, SuperSourceInfo> SuperSources { get; set; } = new();

    /// <summary>
    /// Mix effect configurations and capabilities
    /// </summary>
    public Dictionary<int, MixEffectInfo> MixEffects { get; set; } = new();

    /// <summary>
    /// Device capabilities and hardware configuration
    /// </summary>
    public AtemCapabilities? Capabilities { get; set; }

    /// <summary>
    /// Multiviewer configuration and capabilities
    /// </summary>
    public MultiViewerInfo MultiViewer { get; set; } = new();

    /// <summary>
    /// Supported video modes for this device
    /// </summary>
    public SupportedVideoMode[] SupportedVideoModes { get; set; } = [];
}