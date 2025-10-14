using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public int MixEffectId { get; init; }

    /// <summary>
    /// Rate of the dip transition in frames
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    /// Input source for the dip transition
    /// </summary>
    public int Input { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TransitionDipUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var mixEffectId = rawCommand.ReadUInt8(0);
        var rate = rawCommand.ReadUInt8(1);
        var input = rawCommand.ReadUInt16BigEndian(2);

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
        mixEffect.TransitionSettings ??= new TransitionSettings();

        // Initialize dip settings if not present
        mixEffect.TransitionSettings.Dip ??= new DipTransitionSettings();

        // Update the dip settings
        mixEffect.TransitionSettings.Dip.Rate = Rate;
        mixEffect.TransitionSettings.Dip.Input = Input;
    }
}
