using AtemSharp.Enums;
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
    public int MixEffectId { get; set; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerId { get; set; }

    /// <summary>
    /// Whether the upstream keyer is on air
    /// </summary>
    public bool OnAir { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MixEffectKeyOnAirUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var keyerId = reader.ReadByte();
        var onAir = reader.ReadBoolean();;
        reader.ReadByte(); // Skip padding byte

        return new MixEffectKeyOnAirUpdateCommand
        {
            MixEffectId = mixEffectId,
            KeyerId = keyerId,
            OnAir = onAir
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
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

        // Return the state path that was modified
        return [$"video.mixEffects.{MixEffectId}.upstreamKeyers.{KeyerId}.onAir"];
    }
}