using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition preview update
/// </summary>
[Command("TrPr")]
public partial class PreviewTransitionUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    [DeserializedField(1)]
    private bool _preview;

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

        // Update the transition preview state
        mixEffect.TransitionPreview = Preview;
    }
}
