using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer luma settings update
/// </summary>
[Command("KeLm")]
public class MixEffectKeyLumaUpdateCommand : IDeserializedCommand
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
    /// Whether the key should be treated as premultiplied
    /// </summary>
    public bool PreMultiplied { get; set; }

    /// <summary>
    /// Clip threshold value
    /// </summary>
    public double Clip { get; set; }

    /// <summary>
    /// Gain value for the luma key
    /// </summary>
    public double Gain { get; set; }

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    public bool Invert { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MixEffectKeyLumaUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var keyerId = reader.ReadByte();
        var preMultiplied = reader.ReadBoolean();
        reader.ReadByte(); // Skip padding byte
        
        // Read percentage values as 16-bit integers and convert (divide by 10 to match TypeScript implementation)
        var clip = reader.ReadUInt16BigEndian() / 10.0;
        var gain = reader.ReadUInt16BigEndian() / 10.0;
        
        var invert = reader.ReadBoolean();
        reader.ReadBytes(3); // Skip remaining padding bytes

        return new MixEffectKeyLumaUpdateCommand
        {
            MixEffectId = mixEffectId,
            KeyerId = keyerId,
            PreMultiplied = preMultiplied,
            Clip = clip,
            Gain = gain,
            Invert = invert
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
        
        // Get or create the luma settings
        if (keyer.LumaSettings == null)
            keyer.LumaSettings = new UpstreamKeyerLumaSettings();
        
        // Update the luma settings
        keyer.LumaSettings.PreMultiplied = PreMultiplied;
        keyer.LumaSettings.Clip = Clip;
        keyer.LumaSettings.Gain = Gain;
        keyer.LumaSettings.Invert = Invert;

        // Return the state path that was modified
        return [
            $"video.mixEffects.{MixEffectId}.upstreamKeyers.{KeyerId}.lumaSettings.preMultiplied",
            $"video.mixEffects.{MixEffectId}.upstreamKeyers.{KeyerId}.lumaSettings.clip",
            $"video.mixEffects.{MixEffectId}.upstreamKeyers.{KeyerId}.lumaSettings.gain",
            $"video.mixEffects.{MixEffectId}.upstreamKeyers.{KeyerId}.lumaSettings.invert"
        ];
    }
}