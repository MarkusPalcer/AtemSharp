using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing dip transition settings update
/// </summary>
[Command("TDpP")]
public partial class TransitionDipUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    /// <summary>
    /// Rate of the dip transition in frames
    /// </summary>
    [DeserializedField(1)]
    private byte _rate;

    /// <summary>
    /// Input source for the dip transition
    /// </summary>
    [DeserializedField(2)]
    private ushort _input;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Update the dip settings
        mixEffect.TransitionSettings.Dip.Rate = Rate;
        mixEffect.TransitionSettings.Dip.Input = Input;
    }
}
