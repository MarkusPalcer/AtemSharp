using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer base properties
/// </summary>
[Command("KeBP")]
public class MixEffectKeyPropertiesGetCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectIndex { get; init; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerIndex { get; init; }

    /// <summary>
    /// Type of keying effect
    /// </summary>
    public MixEffectKeyType KeyType { get; init; }

    /// <summary>
    /// Whether this keyer supports fly key functionality
    /// </summary>
    public bool CanFlyKey { get; init; }

    /// <summary>
    /// Whether fly key is currently enabled
    /// </summary>
    public bool FlyEnabled { get; init; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource { get; init; }

    /// <summary>
    /// Cut source input number
    /// </summary>
    public int CutSource { get; init; }

    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool MaskEnabled { get; init; }

    /// <summary>
    /// Top edge of mask
    /// </summary>
    public double MaskTop { get; init; }

    /// <summary>
    /// Bottom edge of mask
    /// </summary>
    public double MaskBottom { get; init; }

    /// <summary>
    /// Left edge of mask
    /// </summary>
    public double MaskLeft { get; init; }

    /// <summary>
    /// Right edge of mask
    /// </summary>
    public double MaskRight { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectKeyPropertiesGetCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MixEffectKeyPropertiesGetCommand
        {
            MixEffectIndex = rawCommand.ReadUInt8(0),
            KeyerIndex = rawCommand.ReadUInt8(1),
            KeyType = (MixEffectKeyType)rawCommand.ReadUInt8(2),
            CanFlyKey = rawCommand.ReadBoolean(4),
            FlyEnabled = rawCommand.ReadBoolean(5),
            FillSource = rawCommand.ReadUInt16BigEndian(6),
            CutSource = rawCommand.ReadUInt16BigEndian(8),
            MaskEnabled = rawCommand.ReadBoolean(10),
            MaskTop = rawCommand.ReadInt16BigEndian(12) / 1000.0,
            MaskBottom = rawCommand.ReadInt16BigEndian(14) / 1000.0,
            MaskLeft = rawCommand.ReadInt16BigEndian(16) / 1000.0,
            MaskRight = rawCommand.ReadInt16BigEndian(18) / 1000.0
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Validate mix effect index - need to get capabilities info
        if (state.Info.Capabilities == null || MixEffectIndex >= state.Info.Capabilities.MixEffects)
        {
            throw new InvalidIdError("MixEffect", MixEffectIndex);
        }

        // TODO: Add validation for keyer index when capabilities include upstream keyer count
        // For now, we'll proceed with state updates

        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects.GetOrCreate(MixEffectIndex);

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerIndex);
        keyer.Index = KeyerIndex;

        // Update keyer properties
        keyer.KeyType = KeyType;
        keyer.CanFlyKey = CanFlyKey;
        keyer.FlyEnabled = FlyEnabled;
        keyer.FillSource = FillSource;
        keyer.CutSource = CutSource;

        // Update mask settings
        keyer.MaskSettings ??= new UpstreamKeyerMaskSettings();

        keyer.MaskSettings.MaskEnabled = MaskEnabled;
        keyer.MaskSettings.MaskTop = MaskTop;
        keyer.MaskSettings.MaskBottom = MaskBottom;
        keyer.MaskSettings.MaskLeft = MaskLeft;
        keyer.MaskSettings.MaskRight = MaskRight;
    }
}
