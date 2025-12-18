using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// Upstream keyer state containing key properties and settings
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class UpstreamKeyer
{
    public byte MixEffectId { get; internal init; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public byte Id { get; internal init; }

    /// <summary>
    /// Whether the keyer is currently on air
    /// </summary>
    public bool OnAir { get; internal set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public ushort FillSource { get; internal set; }

    /// <summary>
    /// Cut source input number
    /// </summary>
    public ushort CutSource { get; internal set; }

    /// <summary>
    /// Type of keying effect (Luma, Chroma, Pattern, DVE)
    /// </summary>
    public MixEffectKeyType KeyType { get; internal set; }

    /// <summary>
    /// Whether this keyer supports fly key functionality
    /// </summary>
    public bool CanFlyKey { get; internal set; }

    /// <summary>
    /// Whether fly key is currently enabled
    /// </summary>
    public bool FlyEnabled { get; internal set; }

    /// <summary>
    /// Mask configuration
    /// </summary>
    public MaskProperties Mask { get; } = new();

    /// <summary>
    /// The settings for the pre multiplied key
    /// </summary>
    /// <remarks>
    /// Only used for Luma
    /// </remarks>
    public PreMultipliedKey PreMultipliedKey { get; } = new();

    /// <summary>
    /// Advanced chroma key settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerAdvancedChromaSettings AdvancedChromaSettings { get; } = new();

    /// <summary>
    /// DVE (Digital Video Effects) settings for the upstream keyer
    /// </summary>
    public UpstreamKeyerDigitalVideoEffectSettings DigitalVideoEffectsSettings { get; } = new();

    /// <summary>
    /// Fly properties for the upstream keyer
    /// </summary>
    public UpstreamKeyerFlyProperties FlyProperties { get; } = new();

    public UpstreamKeyerFlyKeyframe[] Keyframes { get; }

    public UpstreamKeyerPatternProperties Pattern { get; } = new();

    public UpstreamKeyer()
    {
        Keyframes =
        [
            new UpstreamKeyerFlyKeyframe
            {
                Id = 1,
                MixEffectId = MixEffectId,
                UpstreamKeyerId = Id
            },
            new UpstreamKeyerFlyKeyframe
            {
                Id = 2,
                MixEffectId = MixEffectId,
                UpstreamKeyerId = Id
            }
        ];
    }
}
