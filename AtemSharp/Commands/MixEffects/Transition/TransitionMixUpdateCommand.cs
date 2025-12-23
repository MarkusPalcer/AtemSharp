using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TMxP")]
internal partial class TransitionMixUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;
    [DeserializedField(1)] private byte _rate;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.MixEffects[_mixEffectId].TransitionSettings.Mix.Rate = Rate;
    }
}
