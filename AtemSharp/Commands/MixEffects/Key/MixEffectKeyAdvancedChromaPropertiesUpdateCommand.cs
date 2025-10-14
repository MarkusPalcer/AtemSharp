using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer advanced chroma properties update
/// </summary>
[Command("KACk")]
public class MixEffectKeyAdvancedChromaPropertiesUpdateCommand : IDeserializedCommand
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
    /// Foreground level value
    /// </summary>
    public double ForegroundLevel { get; init; }

    /// <summary>
    /// Background level value
    /// </summary>
    public double BackgroundLevel { get; init; }

    /// <summary>
    /// Key edge value
    /// </summary>
    public double KeyEdge { get; init; }

    /// <summary>
    /// Spill suppression value
    /// </summary>
    public double SpillSuppression { get; init; }

    /// <summary>
    /// Flare suppression value
    /// </summary>
    public double FlareSuppression { get; init; }

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    public double Brightness { get; init; }

    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    public double Contrast { get; init; }

    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    public double Saturation { get; init; }

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    public double Red { get; init; }

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    public double Green { get; init; }

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    public double Blue { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectKeyAdvancedChromaPropertiesUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        // Read all property values as 16-bit integers and divide by 10 (to match TypeScript implementation scaling)

        // Color adjustment values can be negative, so use signed 16-bit (also scaled by 10)

        return new MixEffectKeyAdvancedChromaPropertiesUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            KeyerId = rawCommand.ReadUInt8(1),
            ForegroundLevel = rawCommand.ReadUInt16BigEndian(2) / 10.0,
            BackgroundLevel = rawCommand.ReadUInt16BigEndian(4) / 10.0,
            KeyEdge = rawCommand.ReadUInt16BigEndian(6) / 10.0,
            SpillSuppression = rawCommand.ReadUInt16BigEndian(8) / 10.0,
            FlareSuppression = rawCommand.ReadUInt16BigEndian(10) / 10.0,
            Brightness = rawCommand.ReadInt16BigEndian(12) / 10.0,
            Contrast = rawCommand.ReadInt16BigEndian(14) / 10.0,
            Saturation = rawCommand.ReadUInt16BigEndian(16) / 10.0,
            Red = rawCommand.ReadInt16BigEndian(18) / 10.0,
            Green = rawCommand.ReadInt16BigEndian(20) / 10.0,
            Blue = rawCommand.ReadInt16BigEndian(22) / 10.0
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

        // Get or create the advanced chroma settings
        keyer.AdvancedChromaSettings ??= new UpstreamKeyerAdvancedChromaSettings();

        keyer.AdvancedChromaSettings.Properties ??= new UpstreamKeyerAdvancedChromaProperties();

        // Update the advanced chroma properties
        var properties = keyer.AdvancedChromaSettings.Properties;
        properties.ForegroundLevel = ForegroundLevel;
        properties.BackgroundLevel = BackgroundLevel;
        properties.KeyEdge = KeyEdge;
        properties.SpillSuppression = SpillSuppression;
        properties.FlareSuppression = FlareSuppression;
        properties.Brightness = Brightness;
        properties.Contrast = Contrast;
        properties.Saturation = Saturation;
        properties.Red = Red;
        properties.Green = Green;
        properties.Blue = Blue;
    }
}
