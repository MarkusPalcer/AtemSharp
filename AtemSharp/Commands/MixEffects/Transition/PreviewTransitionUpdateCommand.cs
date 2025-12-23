using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TrPr")]
internal partial class PreviewTransitionUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private bool _preview;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.MixEffects[MixEffectId].TransitionPreview = Preview;
    }
}
