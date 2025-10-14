using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition properties update
/// </summary>
[Command("TrSS")]
public class TransitionPropertiesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; init; }

    /// <summary>
    /// If in a transition, this is the style of the running transition.
    /// If no transition is active it will mirror NextStyle
    /// </summary>
    public TransitionStyle Style { get; init; }

    /// <summary>
    /// If in a transition, this is the selection of the running transition.
    /// If no transition is active it will mirror NextSelection
    /// </summary>
    public TransitionSelection Selection { get; init; }

    /// <summary>
    /// The style for the next transition
    /// </summary>
    public TransitionStyle NextStyle { get; init; }

    /// <summary>
    /// The selection for the next transition
    /// </summary>
    public TransitionSelection NextSelection { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TransitionPropertiesUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new TransitionPropertiesUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            Style = (TransitionStyle)rawCommand.ReadUInt8(1),
            Selection = (TransitionSelection)rawCommand.ReadUInt8(2),
            NextStyle = (TransitionStyle)rawCommand.ReadUInt8(3),
            NextSelection = (TransitionSelection)rawCommand.ReadUInt8(4)
        };
    }

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

        // Create or update transition properties
        mixEffect.TransitionProperties ??= new TransitionProperties();

        // Update the transition properties
        mixEffect.TransitionProperties.Style = Style;
        mixEffect.TransitionProperties.Selection = Selection;
        mixEffect.TransitionProperties.NextStyle = NextStyle;
        mixEffect.TransitionProperties.NextSelection = NextSelection;
    }
}
