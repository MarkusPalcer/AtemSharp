using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Device information and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class DeviceInfo
{
    /// <summary>
    /// Product identifier string from the device
    /// </summary>
    public string? ProductIdentifier { get; internal set; }

    /// <summary>
    /// ATEM device model
    /// </summary>
    public Model Model { get; internal set; }

    /// <summary>
    /// ATEM protocol API version
    /// </summary>
    public ProtocolVersion ApiVersion { get; internal set; } = ProtocolVersion.Unknown;

    /// <summary>
    /// Power supply status for each power supply in the device
    /// </summary>
    public bool[] Power { get; internal set; } = [];

    public MixerInfo? Mixer { get; internal set; }

    /// <summary>
    /// Macro pool configuration and capabilities
    /// </summary>
    public MacroPoolInfo MacroPool { get; } = new();

    /// <summary>
    /// Media pool configuration and capabilities
    /// </summary>
    public MediaPoolInfo MediaPool { get; } = new();

    /// <summary>
    /// SuperSource configurations and capabilities
    /// </summary>
    public SuperSourceInfo[] SuperSources { get; internal set; } = [];

    /// <summary>
    /// Mix effect configurations and capabilities
    /// </summary>
    public MixEffectInfo[] MixEffects { get; internal set; } = [];

    /// <summary>
    /// Device capabilities and hardware configuration
    /// </summary>
    public AtemCapabilities Capabilities { get; } = new();

    /// <summary>
    /// Multiviewer configuration and capabilities
    /// </summary>
    public MultiViewerInfo MultiViewer { get; } = new();

    /// <summary>
    /// Supported video modes for this device
    /// </summary>
    public SupportedVideoMode[] SupportedVideoModes { get; internal set; } = [];
}
