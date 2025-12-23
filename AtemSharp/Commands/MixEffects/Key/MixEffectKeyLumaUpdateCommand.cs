using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("KeLm")]
internal partial class MixEffectKeyLumaUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _keyerId;

    [DeserializedField(2)] private bool _preMultiplied;

    [DeserializedField(4)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _clip;

    [DeserializedField(6)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _gain;

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    [DeserializedField(8)] private bool _invert;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[MixEffectId];
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);

        keyer.PreMultipliedKey.Enabled = PreMultiplied;
        keyer.PreMultipliedKey.Clip = Clip;
        keyer.PreMultipliedKey.Gain = Gain;
        keyer.PreMultipliedKey.Inverted = Invert;
    }
}
