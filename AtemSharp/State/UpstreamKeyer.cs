using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Upstream keyer state containing key properties and settings
/// </summary>
public class UpstreamKeyer
{
    public byte MixEffectId { get; internal set; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public byte Id { get; set; }

    /// <summary>
    /// Whether the keyer is currently on air
    /// </summary>
    public bool OnAir { get; set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource { get; set; }

    /// <summary>
    /// Cut source input number
    /// </summary>
    public int CutSource { get; set; }

    /// <summary>
    /// Type of keying effect (Luma, Chroma, Pattern, DVE)
    /// </summary>
    public MixEffectKeyType KeyType { get; set; }

    /// <summary>
    /// Whether this keyer supports fly key functionality
    /// </summary>
    public bool CanFlyKey { get; set; }

    /// <summary>
    /// Whether fly key is currently enabled
    /// </summary>
    public bool FlyEnabled { get; set; }

    /// <summary>
    /// Mask settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerMaskSettings MaskSettings { get; } = new();

    /// <summary>
    /// Luma key settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerLumaSettings LumaSettings { get; } = new();

    /// <summary>
    /// Advanced chroma key settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerAdvancedChromaSettings AdvancedChromaSettings { get; } = new();

    /// <summary>
    /// DVE (Digital Video Effects) settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerDVESettings DigitalVideoEffectsSettings { get; } = new();

    /// <summary>
    /// Fly properties for the upstream keyer
    /// </summary>
    public UpstreamKeyerFlyProperties FlyProperties { get; } = new();
}
