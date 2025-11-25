using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TrPs")]
public partial class TransitionPositionUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    /// <summary>
    /// Whether a transition is currently in progress
    /// </summary>
    [DeserializedField(1)] private bool _inTransition;

    /// <summary>
    /// Number of frames remaining in the transition
    /// </summary>
    [DeserializedField(2)] private byte _remainingFrames;

    /// <summary>
    /// Current position of the transition handle (0.0 to 1.0)
    /// </summary>
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
