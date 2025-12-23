using AtemSharp.State;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Commands.MixEffects.Transition;

[Command("TrSS")]
internal partial class TransitionPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private TransitionStyle _style;

    [DeserializedField(2)] private TransitionSelection _selection;

    [DeserializedField(3)] private TransitionStyle _nextStyle;

    [DeserializedField(4)] private TransitionSelection _nextSelection;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var mixEffect = state.Video.MixEffects[_mixEffectId];
        mixEffect.TransitionProperties.Style = Style;
        mixEffect.TransitionProperties.Selection = Selection;
        mixEffect.TransitionProperties.NextStyle = NextStyle;
        mixEffect.TransitionProperties.NextSelection = NextSelection;
    }
}
