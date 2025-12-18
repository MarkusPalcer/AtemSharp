using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Info;

/// <summary>
/// Device information and capabilities
/// </summary>
public class DeviceInfo
{
    internal DeviceInfo()
    {
        MixEffects = new ItemCollection<byte, MixEffectInfo>(id => new MixEffectInfo { Id = id });
        SuperSources = new ItemCollection<byte, SuperSourceInfo>(id => new SuperSourceInfo { Id = id });
    }

    /// <summary>
    /// Product identifier string from the device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public string? ProductIdentifier { get; internal set; }

    /// <summary>
    /// ATEM device model
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public Model Model { get; internal set; }

    /// <summary>
    /// ATEM protocol API version
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ProtocolVersion ApiVersion { get; internal set; } = ProtocolVersion.Unknown;

    /// <summary>
    /// Power supply status for each power supply in the device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public bool[] Power { get; internal set; } = [];


    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MixerInfo? Mixer { get; internal set; }

    /// <summary>
    /// Macro pool configuration and capabilities
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MacroPoolInfo MacroPool { get; } = new();

    /// <summary>
    /// Media pool configuration and capabilities
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MediaPoolInfo MediaPool { get; } = new();

    /// <summary>
    /// SuperSource configurations and capabilities
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, SuperSourceInfo> SuperSources { get; }

    /// <summary>
    /// Mix effect configurations and capabilities
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, MixEffectInfo> MixEffects { get; }

    /// <summary>
    /// Device capabilities and hardware configuration
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public AtemCapabilities Capabilities { get; } = new();

    /// <summary>
    /// Multiviewer configuration and capabilities
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public MultiViewerInfo MultiViewer { get; } = new();

    /// <summary>
    /// Supported video modes for this device
    /// </summary>
    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public SupportedVideoMode[] SupportedVideoModes { get; internal set; } = [];
}
