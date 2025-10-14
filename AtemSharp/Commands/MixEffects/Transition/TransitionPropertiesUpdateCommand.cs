using AtemSharp.Enums;
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
    public int MixEffectId { get; set; }

    /// <summary>
    /// If in a transition, this is the style of the running transition.
    /// If no transition is active it will mirror NextStyle
    /// </summary>
    public TransitionStyle Style { get; set; }

    /// <summary>
    /// If in a transition, this is the selection of the running transition.
    /// If no transition is active it will mirror NextSelection
    /// </summary>
    public TransitionSelection Selection { get; set; }

    /// <summary>
    /// The style for the next transition
    /// </summary>
    public TransitionStyle NextStyle { get; set; }

    /// <summary>
    /// The selection for the next transition
    /// </summary>
    public TransitionSelection NextSelection { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TransitionPropertiesUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var style = (TransitionStyle)reader.ReadByte();
        var selection = (TransitionSelection)reader.ReadByte();
        var nextStyle = (TransitionStyle)reader.ReadByte();
        var nextSelection = (TransitionSelection)reader.ReadByte();

        return new TransitionPropertiesUpdateCommand
        {
            MixEffectId = mixEffectId,
            Style = style,
            Selection = selection,
            NextStyle = nextStyle,
            NextSelection = nextSelection
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
        if (mixEffect.TransitionProperties == null)
        {
            mixEffect.TransitionProperties = new TransitionProperties();
        }

        // Update the transition properties
        mixEffect.TransitionProperties.Style = Style;
        mixEffect.TransitionProperties.Selection = Selection;
        mixEffect.TransitionProperties.NextStyle = NextStyle;
        mixEffect.TransitionProperties.NextSelection = NextSelection;
    }
}
