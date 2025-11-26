using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.FadeToBlack;

[Command("FtbS")]
public partial class FadeToBlackStateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private bool _isFullyBlack;

    [DeserializedField(2)] private bool _inTransition;

    [DeserializedField(3)] private byte _remainingFrames;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[MixEffectId];
        mixEffect.FadeToBlack.IsFullyBlack = IsFullyBlack;
        mixEffect.FadeToBlack.InTransition = InTransition;
        mixEffect.FadeToBlack.RemainingFrames = RemainingFrames;
    }
}
