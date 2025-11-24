using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition mix settings update
/// </summary>
[Command("TMxP")]
public partial class TransitionMixUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    [NoProperty]
    internal byte MixEffectId;

    /// <summary>
    /// Rate of the mix transition in frames (0-250)
    /// </summary>
    [DeserializedField(1)]
    private byte _rate;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Update the mix rate
        mixEffect.TransitionSettings.Mix.Rate = Rate;
    }
}
