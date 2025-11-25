using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition preview update
/// </summary>
[Command("TrPr")]
public partial class PreviewTransitionUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private bool _preview;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Video.MixEffects[MixEffectId].TransitionPreview = Preview;
    }
}
