using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbP")]
public partial class FadeToBlackRateUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _rate;

    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[MixEffectId];
        mixEffect.FadeToBlack ??= new FadeToBlackProperties();
        mixEffect.FadeToBlack.Rate = Rate;
        mixEffect.FadeToBlack.IsFullyBlack = false;
        mixEffect.FadeToBlack.InTransition = false;
        mixEffect.FadeToBlack.RemainingFrames = 0;
    }
}
