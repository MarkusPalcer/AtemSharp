using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command received from ATEM device containing transition mix settings update
/// </summary>
[Command("TMxP")]
public class TransitionMixUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; init; }

    /// <summary>
    /// Rate of the mix transition in frames (0-250)
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static TransitionMixUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var mixEffectId = rawCommand.ReadUInt8(0);
        var rate = rawCommand.ReadUInt8(1);

        return new TransitionMixUpdateCommand
        {
            MixEffectId = mixEffectId,
            Rate = rate
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

        // Initialize mix settings if not present
        mixEffect.TransitionSettings.Mix ??= new MixTransitionSettings();

        // Update the mix rate
        mixEffect.TransitionSettings.Mix.Rate = Rate;
    }
}
