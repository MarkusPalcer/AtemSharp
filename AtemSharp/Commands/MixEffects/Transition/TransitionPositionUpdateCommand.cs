using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TrPs")]
public partial class TransitionPositionUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    [NoProperty]
    internal byte MixEffectId;

    /// <summary>
    /// Whether a transition is currently in progress
    /// </summary>
    [DeserializedField(1)]
    private bool _inTransition;

    /// <summary>
    /// Number of frames remaining in the transition
    /// </summary>
    [DeserializedField(2)]
    private byte _remainingFrames;

    /// <summary>
    /// Current position of the transition handle (0.0 to 1.0)
    /// </summary>
    [DeserializedField(4)]
    [ScalingFactor(10000)]
    [SerializedType(typeof(ushort))]
    private double _handlePosition;


    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (state.Info.Capabilities == null || MixEffectId >= state.Info.Capabilities.MixEffects)
        {
            throw new InvalidIdError("MixEffect", MixEffectId);
        }

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectId);

        // Update the transition position
        mixEffect.TransitionPosition.InTransition = InTransition;
        mixEffect.TransitionPosition.RemainingFrames = RemainingFrames;
        mixEffect.TransitionPosition.HandlePosition = HandlePosition;
    }
}
