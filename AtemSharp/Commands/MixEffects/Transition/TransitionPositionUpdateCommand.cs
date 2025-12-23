using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TrPs")]
internal partial class TransitionPositionUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private bool _inTransition;

    [DeserializedField(2)] private byte _remainingFrames;

    [DeserializedField(4)] [ScalingFactor(10000)] [SerializedType(typeof(ushort))]
    private double _handlePosition;


    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[_mixEffectId];
        mixEffect.TransitionPosition.InTransition = InTransition;
        mixEffect.TransitionPosition.RemainingFrames = RemainingFrames;
        mixEffect.TransitionPosition.HandlePosition = HandlePosition;
    }
}
