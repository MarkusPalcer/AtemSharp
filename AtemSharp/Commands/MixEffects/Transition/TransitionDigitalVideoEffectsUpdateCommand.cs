using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TDvP")]
internal partial class TransitionDigitalVideoEffectsUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _rate;

    [DeserializedField(2)] private byte _logoRate;

    [DeserializedField(3)] private DigitalVideoEffect _style;

    [DeserializedField(4)] private ushort _fillSource;

    [DeserializedField(6)] private ushort _keySource;

    [DeserializedField(8)] private bool _enableKey;

    [DeserializedField(9)] private bool _preMultiplied;

    [DeserializedField(10)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _clip;

    [DeserializedField(12)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _gain;

    [DeserializedField(14)] private bool _invertKey;

    [DeserializedField(15)] private bool _reverse;

    [DeserializedField(16)] private bool _flipFlop;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[_mixEffectId];
        mixEffect.TransitionSettings.DigitalVideoEffect.Rate = Rate;
        mixEffect.TransitionSettings.DigitalVideoEffect.LogoRate = LogoRate;
        mixEffect.TransitionSettings.DigitalVideoEffect.Style = Style;
        mixEffect.TransitionSettings.DigitalVideoEffect.FillSource = FillSource;
        mixEffect.TransitionSettings.DigitalVideoEffect.KeySource = KeySource;
        mixEffect.TransitionSettings.DigitalVideoEffect.EnableKey = EnableKey;
        mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Enabled = PreMultiplied;
        mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Clip = Clip;
        mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Gain = Gain;
        mixEffect.TransitionSettings.DigitalVideoEffect.PreMultipliedKey.Inverted = InvertKey;
        mixEffect.TransitionSettings.DigitalVideoEffect.Reverse = Reverse;
        mixEffect.TransitionSettings.DigitalVideoEffect.FlipFlop = FlipFlop;
    }
}
