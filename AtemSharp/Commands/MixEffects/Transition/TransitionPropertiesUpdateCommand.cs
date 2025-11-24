using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition properties update
/// </summary>
[Command("TrSS")]
public partial class TransitionPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _mixEffectId;

    /// <summary>
    /// If in a transition, this is the style of the running transition.
    /// If no transition is active it will mirror NextStyle
    /// </summary>
    [DeserializedField(1)]
    private TransitionStyle _style;

    /// <summary>
    /// If in a transition, this is the selection of the running transition.
    /// If no transition is active it will mirror NextSelection
    /// </summary>
    [DeserializedField(2)]
    private TransitionSelection _selection;

    /// <summary>
    /// The style for the next transition
    /// </summary>
    [DeserializedField(3)]
    private TransitionStyle _nextStyle;

    /// <summary>
    /// The selection for the next transition
    /// </summary>
    [DeserializedField(4)]
    private TransitionSelection _nextSelection;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (state.Info.Capabilities == null || MixEffectId >= state.Info.Capabilities.MixEffects)
        {
            throw new InvalidIdError("MixEffect", MixEffectId);
        }

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectId);

        // Update the transition properties
        mixEffect.TransitionProperties.Style = Style;
        mixEffect.TransitionProperties.Selection = Selection;
        mixEffect.TransitionProperties.NextStyle = NextStyle;
        mixEffect.TransitionProperties.NextSelection = NextSelection;
    }
}
