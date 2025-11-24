using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbS")]
public partial class FadeToBlackStateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    [DeserializedField(1)]
    private bool _isFullyBlack;

    [DeserializedField(2)]
    private bool _inTransition;

    [DeserializedField(3)]
    private byte _remainingFrames;

    public void ApplyToState(AtemState state)
    {
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
        mixEffect.FadeToBlack.IsFullyBlack = IsFullyBlack;
        mixEffect.FadeToBlack.InTransition = InTransition;
        mixEffect.FadeToBlack.RemainingFrames = RemainingFrames;
    }
}
