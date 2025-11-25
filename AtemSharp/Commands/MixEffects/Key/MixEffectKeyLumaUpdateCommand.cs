using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer luma settings update
/// </summary>
[Command("KeLm")]
public partial class MixEffectKeyLumaUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _keyerId;

    /// <summary>
    /// Whether the key should be treated as premultiplied
    /// </summary>
    [DeserializedField(2)] private bool _preMultiplied;

    /// <summary>
    /// Clip threshold value
    /// </summary>
    [DeserializedField(4)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _clip;

    /// <summary>
    /// Gain value for the luma key
    /// </summary>
    [DeserializedField(6)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _gain;

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    [DeserializedField(8)] private bool _invert;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectId];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);
        keyer.Id = KeyerId;

        // Update the luma settings
        keyer.LumaSettings.PreMultiplied = PreMultiplied;
        keyer.LumaSettings.Clip = Clip;
        keyer.LumaSettings.Gain = Gain;
        keyer.LumaSettings.Invert = Invert;
    }
}
