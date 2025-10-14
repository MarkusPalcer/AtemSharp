using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer on-air state update
/// </summary>
[Command("KeOn")]
public class MixEffectKeyOnAirUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; init; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerId { get; init; }

    /// <summary>
    /// Whether the upstream keyer is on air
    /// </summary>
    public bool OnAir { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectKeyOnAirUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MixEffectKeyOnAirUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            KeyerId = rawCommand.ReadUInt8(1),
            OnAir = rawCommand.ReadBoolean(2)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index - need to get capabilities info
        if (state.Info.Capabilities == null || MixEffectId >= state.Info.Capabilities.MixEffects)
        {
            throw new InvalidIdError("MixEffect", MixEffectId);
        }

        // TODO: Add validation for keyer index when capabilities include upstream keyer count
        // For now, we'll proceed with state updates

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectId);

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);
        keyer.Index = KeyerId;

        // Update the on-air state
        keyer.OnAir = OnAir;
    }
}
