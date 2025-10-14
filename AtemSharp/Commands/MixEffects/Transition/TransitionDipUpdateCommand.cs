using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing dip transition settings update
/// </summary>
[Command("TDpP")]
public class TransitionDipUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; set; }

    /// <summary>
    /// Rate of the dip transition in frames
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    /// Input source for the dip transition
    /// </summary>
    public int Input { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static TransitionDipUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var rate = reader.ReadByte();
        var input = reader.ReadUInt16BigEndian();

        return new TransitionDipUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = rate,
            Input = input
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index
        if (!state.Video.MixEffects.TryGetValue(MixEffectId, out var mixEffect))
        {
            throw new InvalidIdError("MixEffect", MixEffectId.ToString());
        }

        // Initialize transition settings if not present
        if (mixEffect.TransitionSettings == null)
        {
            mixEffect.TransitionSettings = new TransitionSettings();
        }

        // Initialize dip settings if not present
        if (mixEffect.TransitionSettings.Dip == null)
        {
            mixEffect.TransitionSettings.Dip = new DipTransitionSettings();
        }

        // Update the dip settings
        mixEffect.TransitionSettings.Dip.Rate = Rate;
        mixEffect.TransitionSettings.Dip.Input = Input;
    }
}
