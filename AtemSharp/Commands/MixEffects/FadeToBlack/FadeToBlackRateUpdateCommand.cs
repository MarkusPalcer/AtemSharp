using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbP")]
public partial class FadeToBlackRateUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    [DeserializedField(1)]
    private byte _rate;

    public void ApplyToState(AtemState state)
    {
        // TODO: Change to array access after array initializes from Topology command
        if (state.Info.Capabilities is null)
        {
            throw new InvalidOperationException("Fade to black rate cannot be applied before capabilities are known");
        }

        if (MixEffectId >= state.Info.Capabilities.MixEffects)
        {
            throw new IndexOutOfRangeException("Mix effect with index {MixEffectId} does not exist");
        }

        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectId);
        mixEffect.Index = MixEffectId;
        mixEffect.FadeToBlack ??= new FadeToBlackProperties();
        mixEffect.FadeToBlack.Rate = Rate;
        mixEffect.FadeToBlack.IsFullyBlack = false;
        mixEffect.FadeToBlack.InTransition = false;
        mixEffect.FadeToBlack.RemainingFrames = 0;
    }
}
