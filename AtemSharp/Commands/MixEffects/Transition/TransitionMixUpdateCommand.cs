using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition mix settings update
/// </summary>
[Command("TMxP")]
public partial class TransitionMixUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    /// <summary>
    /// Rate of the mix transition in frames (0-250)
    /// </summary>
    [DeserializedField(1)] private byte _rate;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.MixEffects[_mixEffectId].TransitionSettings.Mix.Rate = Rate;
    }
}
