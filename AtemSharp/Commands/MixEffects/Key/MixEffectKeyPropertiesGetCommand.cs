using AtemSharp.Enums;
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
    public int MixEffectIndex { get; set; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerIndex { get; set; }

    /// <summary>
    /// Type of keying effect
    /// </summary>
    public MixEffectKeyType KeyType { get; set; }

    /// <summary>
    /// Whether this keyer supports fly key functionality
    /// </summary>
    public bool CanFlyKey { get; set; }

    /// <summary>
    /// Whether fly key is currently enabled
    /// </summary>
    public bool FlyEnabled { get; set; }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource { get; set; }

    /// <summary>
    /// Cut source input number
    /// </summary>
    public int CutSource { get; set; }

    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool MaskEnabled { get; set; }

    /// <summary>
    /// Top edge of mask
    /// </summary>
    public double MaskTop { get; set; }

    /// <summary>
    /// Bottom edge of mask
    /// </summary>
    public double MaskBottom { get; set; }

    /// <summary>
    /// Left edge of mask
    /// </summary>
    public double MaskLeft { get; set; }

    /// <summary>
    /// Right edge of mask
    /// </summary>
    public double MaskRight { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MixEffectKeyPropertiesGetCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectIndex = reader.ReadByte();           // byte 0
        var keyerIndex = reader.ReadByte();               // byte 1
        var keyType = (MixEffectKeyType)reader.ReadByte(); // byte 2
        reader.ReadByte();                                // byte 3 - padding
        var canFlyKey = reader.ReadBoolean();             // byte 4
        var flyEnabled = reader.ReadBoolean();            // byte 5
        var fillSource = reader.ReadUInt16BigEndian();    // bytes 6-7
        var cutSource = reader.ReadUInt16BigEndian();     // bytes 8-9
        var maskEnabled = reader.ReadBoolean();           // byte 10
        reader.ReadByte();                                // byte 11 - padding
        var maskTop = reader.ReadInt16BigEndian() / 1000.0;     // bytes 12-13
        var maskBottom = reader.ReadInt16BigEndian() / 1000.0;  // bytes 14-15
        var maskLeft = reader.ReadInt16BigEndian() / 1000.0;    // bytes 16-17
        var maskRight = reader.ReadInt16BigEndian() / 1000.0;   // bytes 18-19

        return new MixEffectKeyPropertiesGetCommand
        {
            MixEffectIndex = mixEffectIndex,
            KeyerIndex = keyerIndex,
            KeyType = keyType,
            CanFlyKey = canFlyKey,
            FlyEnabled = flyEnabled,
            FillSource = fillSource,
            CutSource = cutSource,
            MaskEnabled = maskEnabled,
            MaskTop = maskTop,
            MaskBottom = maskBottom,
            MaskLeft = maskLeft,
            MaskRight = maskRight
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
        if (keyer.MaskSettings == null)
            keyer.MaskSettings = new UpstreamKeyerMaskSettings();

        keyer.MaskSettings.MaskEnabled = MaskEnabled;
        keyer.MaskSettings.MaskTop = MaskTop;
        keyer.MaskSettings.MaskBottom = MaskBottom;
        keyer.MaskSettings.MaskLeft = MaskLeft;
        keyer.MaskSettings.MaskRight = MaskRight;
    }
}
