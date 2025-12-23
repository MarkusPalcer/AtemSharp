using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TStP")]
internal partial class TransitionStingerUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _source;

    [DeserializedField(2)] private bool _preMultipliedKey;

    [DeserializedField(4)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _clip;

    [DeserializedField(6)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _gain;

    [DeserializedField(8)] private bool _invert;

    [DeserializedField(10)] private ushort _preroll;

    [DeserializedField(12)] private ushort _clipDuration;

    [DeserializedField(14)] private ushort _triggerPoint;

    [DeserializedField(16)] private ushort _mixRate;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[_mixEffectId];
        mixEffect.TransitionSettings.Stinger.Source = Source;
        mixEffect.TransitionSettings.Stinger.PreMultipliedKey.Enabled = PreMultipliedKey;
        mixEffect.TransitionSettings.Stinger.PreMultipliedKey.Clip = Clip;
        mixEffect.TransitionSettings.Stinger.PreMultipliedKey.Gain = Gain;
        mixEffect.TransitionSettings.Stinger.PreMultipliedKey.Inverted = Invert;
        mixEffect.TransitionSettings.Stinger.Preroll = Preroll;
        mixEffect.TransitionSettings.Stinger.ClipDuration = ClipDuration;
        mixEffect.TransitionSettings.Stinger.TriggerPoint = TriggerPoint;
        mixEffect.TransitionSettings.Stinger.MixRate = MixRate;
    }
}
