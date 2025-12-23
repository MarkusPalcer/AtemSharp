using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TDpP")]
internal partial class TransitionDipUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;
    [DeserializedField(1)] private byte _rate;
    [DeserializedField(2)] private ushort _input;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[_mixEffectId];
        mixEffect.TransitionSettings.Dip.Rate = Rate;
        mixEffect.TransitionSettings.Dip.Input = Input;
    }
}
