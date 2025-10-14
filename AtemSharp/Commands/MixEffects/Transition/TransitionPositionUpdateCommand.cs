using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition position update
/// </summary>
[Command("TrPs")]
public class TransitionPositionUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; init; }

    /// <summary>
    /// Whether a transition is currently in progress
    /// </summary>
    public bool InTransition { get; init; }

    /// <summary>
    /// Number of frames remaining in the transition
    /// </summary>
    public int RemainingFrames { get; init; }

    /// <summary>
    /// Current position of the transition handle (0.0 to 1.0)
    /// </summary>
    public double HandlePosition { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TransitionPositionUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new TransitionPositionUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            InTransition = rawCommand.ReadBoolean(1),
            RemainingFrames = rawCommand.ReadUInt8(2),
            HandlePosition = rawCommand.ReadUInt16BigEndian(4) / 10000.0 // Convert from ushort (0-10000) to double (0.0-1.0)
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

        // Update the transition position
        mixEffect.TransitionPosition.InTransition = InTransition;
        mixEffect.TransitionPosition.RemainingFrames = RemainingFrames;
        mixEffect.TransitionPosition.HandlePosition = HandlePosition;
    }
}
