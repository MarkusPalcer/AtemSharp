using AtemSharp.Enums;
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
    public int MixEffectId { get; set; }

    /// <summary>
    /// Whether a transition is currently in progress
    /// </summary>
    public bool InTransition { get; set; }

    /// <summary>
    /// Number of frames remaining in the transition
    /// </summary>
    public int RemainingFrames { get; set; }

    /// <summary>
    /// Current position of the transition handle (0.0 to 1.0)
    /// </summary>
    public double HandlePosition { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TransitionPositionUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var inTransition = reader.ReadBoolean();
        var remainingFrames = reader.ReadByte();
        reader.ReadByte(); // Skip 1 byte padding
        var handlePosition = reader.ReadUInt16BigEndian();

        return new TransitionPositionUpdateCommand
        {
            MixEffectId = mixEffectId,
            InTransition = inTransition,
            RemainingFrames = remainingFrames,
            HandlePosition = handlePosition / 10000.0 // Convert from ushort (0-10000) to double (0.0-1.0)
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
