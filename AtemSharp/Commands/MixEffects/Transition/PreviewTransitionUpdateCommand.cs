using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public int MixEffectId { get; init; }

    /// <summary>
    /// Whether transition preview is enabled
    /// </summary>
    public bool Preview { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static PreviewTransitionUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new PreviewTransitionUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            Preview = rawCommand.ReadBoolean(1)
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

        // Update the transition preview state
        mixEffect.TransitionPreview = Preview;
    }
}
