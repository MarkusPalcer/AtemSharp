using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TWpP")]
internal partial class TransitionWipeUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _rate;

    [DeserializedField(2)] private byte _pattern;

    [DeserializedField(4)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _borderWidth;

    [DeserializedField(6)] private ushort _borderInput;

    [DeserializedField(8)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _symmetry;

    [DeserializedField(10)] [SerializedType(typeof(ushort))] [ScalingFactor(100)]
    private double _borderSoftness;

    [DeserializedField(12)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _xPosition;

    [DeserializedField(14)] [SerializedType(typeof(ushort))] [ScalingFactor(10000)]
    private double _yPosition;

    [DeserializedField(16)] private bool _reverseDirection;

    [DeserializedField(17)] private bool _flipFlop;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[MixEffectId];
        mixEffect.TransitionSettings.Wipe.Rate = Rate;
        mixEffect.TransitionSettings.Wipe.Pattern = Pattern;
        mixEffect.TransitionSettings.Wipe.BorderWidth = BorderWidth;
        mixEffect.TransitionSettings.Wipe.BorderInput = BorderInput;
        mixEffect.TransitionSettings.Wipe.Symmetry = Symmetry;
        mixEffect.TransitionSettings.Wipe.BorderSoftness = BorderSoftness;
        mixEffect.TransitionSettings.Wipe.XPosition = XPosition;
        mixEffect.TransitionSettings.Wipe.YPosition = YPosition;
        mixEffect.TransitionSettings.Wipe.ReverseDirection = ReverseDirection;
        mixEffect.TransitionSettings.Wipe.FlipFlop = FlipFlop;
    }
}
