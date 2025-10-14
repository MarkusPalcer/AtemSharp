using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public int MixEffectId { get; init; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerId { get; init; }

    /// <summary>
    /// Whether the key should be treated as premultiplied
    /// </summary>
    public bool PreMultiplied { get; init; }

    /// <summary>
    /// Clip threshold value
    /// </summary>
    public double Clip { get; init; }

    /// <summary>
    /// Gain value for the luma key
    /// </summary>
    public double Gain { get; init; }

    /// <summary>
    /// Whether to invert the luma key
    /// </summary>
    public bool Invert { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectKeyLumaUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MixEffectKeyLumaUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            KeyerId = rawCommand.ReadUInt8(1),
            PreMultiplied = rawCommand.ReadBoolean(2),
            Clip = rawCommand.ReadUInt16BigEndian(4) / 10.0,
            Gain = rawCommand.ReadUInt16BigEndian(6) / 10.0,
            Invert = rawCommand.ReadBoolean(8)
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

        // Get or create the luma settings
        keyer.LumaSettings ??= new UpstreamKeyerLumaSettings();

        // Update the luma settings
        keyer.LumaSettings.PreMultiplied = PreMultiplied;
        keyer.LumaSettings.Clip = Clip;
        keyer.LumaSettings.Gain = Gain;
        keyer.LumaSettings.Invert = Invert;
    }
}
