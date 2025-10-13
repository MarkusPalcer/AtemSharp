using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition preview update
/// </summary>
[Command("TrPr")]
public class PreviewTransitionUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; set; }

    /// <summary>
    /// Whether transition preview is enabled
    /// </summary>
    public bool Preview { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static PreviewTransitionUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var preview = reader.ReadBoolean();;

        return new PreviewTransitionUpdateCommand
        {
            MixEffectId = mixEffectId,
            Preview = preview
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (state.Info.Capabilities == null || MixEffectId >= state.Info.Capabilities.MixEffects)
        {
            throw new InvalidIdError("MixEffect", MixEffectId);
        }

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectId);
        
        // Update the transition preview state
        mixEffect.TransitionPreview = Preview;

        // Return the state path that was modified
        return [$"video.mixEffects.{MixEffectId}.transitionPreview"];
    }
}