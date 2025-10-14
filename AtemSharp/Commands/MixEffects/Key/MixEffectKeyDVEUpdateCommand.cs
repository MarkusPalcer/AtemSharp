using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer DVE settings update
/// </summary>
[Command("KeDV")]
// ReSharper disable once InconsistentNaming Domain Specific Acronym
public class MixEffectKeyDVEUpdateCommand : IDeserializedCommand
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
    /// Horizontal size scale factor
    /// </summary>
    public double SizeX { get; init; }

    /// <summary>
    /// Vertical size scale factor
    /// </summary>
    public double SizeY { get; init; }

    /// <summary>
    /// Horizontal position offset
    /// </summary>
    public double PositionX { get; init; }

    /// <summary>
    /// Vertical position offset
    /// </summary>
    public double PositionY { get; init; }

    /// <summary>
    /// Rotation angle in degrees
    /// </summary>
    public double Rotation { get; init; }

    /// <summary>
    /// Whether border effect is enabled
    /// </summary>
    public bool BorderEnabled { get; init; }

    /// <summary>
    /// Whether shadow effect is enabled
    /// </summary>
    public bool ShadowEnabled { get; init; }

    /// <summary>
    /// Type of border bevel effect
    /// </summary>
    public BorderBevel BorderBevel { get; init; }

    /// <summary>
    /// Outer border width
    /// </summary>
    public double BorderOuterWidth { get; init; }

    /// <summary>
    /// Inner border width
    /// </summary>
    public double BorderInnerWidth { get; init; }

    /// <summary>
    /// Outer border softness
    /// </summary>
    public double BorderOuterSoftness { get; init; }

    /// <summary>
    /// Inner border softness
    /// </summary>
    public double BorderInnerSoftness { get; init; }

    /// <summary>
    /// Border bevel softness
    /// </summary>
    public double BorderBevelSoftness { get; init; }

    /// <summary>
    /// Border bevel position
    /// </summary>
    public double BorderBevelPosition { get; init; }

    /// <summary>
    /// Border opacity
    /// </summary>
    public double BorderOpacity { get; init; }

    /// <summary>
    /// Border color hue
    /// </summary>
    public double BorderHue { get; init; }

    /// <summary>
    /// Border color saturation
    /// </summary>
    public double BorderSaturation { get; init; }

    /// <summary>
    /// Border color luminance
    /// </summary>
    public double BorderLuma { get; init; }

    /// <summary>
    /// Light source direction angle
    /// </summary>
    public double LightSourceDirection { get; init; }

    /// <summary>
    /// Light source altitude
    /// </summary>
    public double LightSourceAltitude { get; init; }

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
    /// Transition rate in frames
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectKeyDVEUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        // Read softness and position values as signed bytes

        // Color values - BorderHue uses complex scaling based on reverse engineering analysis

        // Light source

        // Mask values - convert from fixed point (*100/65536)

        return new MixEffectKeyDVEUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            KeyerId = rawCommand.ReadUInt8(1),
            SizeX = rawCommand.ReadUInt32BigEndian(4) / 1000.0,
            SizeY = rawCommand.ReadUInt32BigEndian(8) / 1000.0,
            PositionX = rawCommand.ReadInt32BigEndian(12) / 1000.0,
            PositionY = rawCommand.ReadInt32BigEndian(16) / 1000.0,
            Rotation = rawCommand.ReadInt32BigEndian(20) / 10.0,
            BorderEnabled = rawCommand.ReadBoolean(24),
            ShadowEnabled = rawCommand.ReadBoolean(25),
            BorderBevel = (BorderBevel)rawCommand.ReadUInt8(26),
            BorderOuterWidth = rawCommand.ReadUInt16BigEndian(28) / 100.0,
            BorderInnerWidth = rawCommand.ReadUInt16BigEndian(30) / 100.0,
            BorderOuterSoftness = rawCommand.ReadInt8(32),
            BorderInnerSoftness = rawCommand.ReadInt8(33),
            BorderBevelSoftness = rawCommand.ReadInt8(34),
            BorderBevelPosition = rawCommand.ReadInt8(35),
            BorderOpacity = rawCommand.ReadInt8(36),
            BorderHue = rawCommand.ReadUInt16BigEndian(38),
            BorderSaturation = rawCommand.ReadUInt16BigEndian(40),
            BorderLuma = rawCommand.ReadUInt16BigEndian(42),
            LightSourceDirection = rawCommand.ReadUInt16BigEndian(44),
            LightSourceAltitude = rawCommand.ReadUInt8(46),
            MaskEnabled = rawCommand.ReadBoolean(47),
            MaskTop = rawCommand.ReadUInt16BigEndian(48),
            MaskBottom = rawCommand.ReadUInt16BigEndian(50),
            MaskLeft = rawCommand.ReadUInt16BigEndian(52),
            MaskRight = rawCommand.ReadUInt16BigEndian(54),
            Rate = rawCommand.ReadUInt8(56)
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

        // Get or create the DVE settings
        if (keyer.DVESettings == null)
            keyer.DVESettings = new UpstreamKeyerDVESettings();

        // Update the DVE settings
        keyer.DVESettings.SizeX = SizeX;
        keyer.DVESettings.SizeY = SizeY;
        keyer.DVESettings.PositionX = PositionX;
        keyer.DVESettings.PositionY = PositionY;
        keyer.DVESettings.Rotation = Rotation;
        keyer.DVESettings.BorderEnabled = BorderEnabled;
        keyer.DVESettings.ShadowEnabled = ShadowEnabled;
        keyer.DVESettings.BorderBevel = BorderBevel;
        keyer.DVESettings.BorderOuterWidth = BorderOuterWidth;
        keyer.DVESettings.BorderInnerWidth = BorderInnerWidth;
        keyer.DVESettings.BorderOuterSoftness = BorderOuterSoftness;
        keyer.DVESettings.BorderInnerSoftness = BorderInnerSoftness;
        keyer.DVESettings.BorderBevelSoftness = BorderBevelSoftness;
        keyer.DVESettings.BorderBevelPosition = BorderBevelPosition;
        keyer.DVESettings.BorderOpacity = BorderOpacity;
        keyer.DVESettings.BorderHue = BorderHue;
        keyer.DVESettings.BorderSaturation = BorderSaturation;
        keyer.DVESettings.BorderLuma = BorderLuma;
        keyer.DVESettings.LightSourceDirection = LightSourceDirection;
        keyer.DVESettings.LightSourceAltitude = LightSourceAltitude;
        keyer.DVESettings.MaskEnabled = MaskEnabled;
        keyer.DVESettings.MaskTop = MaskTop;
        keyer.DVESettings.MaskBottom = MaskBottom;
        keyer.DVESettings.MaskLeft = MaskLeft;
        keyer.DVESettings.MaskRight = MaskRight;
        keyer.DVESettings.Rate = Rate;
    }
}
