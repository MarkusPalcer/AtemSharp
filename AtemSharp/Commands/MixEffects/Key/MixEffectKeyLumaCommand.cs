using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update luma key settings for an upstream keyer
/// </summary>
[Command("CKLm")]
[BufferSize(12)]
public partial class MixEffectKeyLumaCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(2)] [NoProperty] private readonly byte _keyerId = keyer.Id;

    /// <summary>
    /// Whether the key should be treated as premultiplied
    /// </summary>
    [SerializedField(3, 0)] private bool _preMultiplied = keyer.PreMultipliedKey.Enabled;

    /// <summary>
    /// Clip threshold value (0-100)
    /// </summary>
    [SerializedField(4, 1)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _clip = keyer.PreMultipliedKey.Clip;

    /// <summary>
    /// Gain value for the luma key (0-100)
    /// </summary>
    [SerializedField(6, 2)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _gain = keyer.PreMultipliedKey.Gain;

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    [SerializedField(8, 3)] private bool _invert = keyer.PreMultipliedKey.Inverted;
}
