using AtemSharp.Enums;
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
    public int MixEffectId { get; set; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerId { get; set; }

    /// <summary>
    /// Foreground level value
    /// </summary>
    public double ForegroundLevel { get; set; }

    /// <summary>
    /// Background level value
    /// </summary>
    public double BackgroundLevel { get; set; }

    /// <summary>
    /// Key edge value
    /// </summary>
    public double KeyEdge { get; set; }

    /// <summary>
    /// Spill suppression value
    /// </summary>
    public double SpillSuppression { get; set; }

    /// <summary>
    /// Flare suppression value
    /// </summary>
    public double FlareSuppression { get; set; }

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    public double Brightness { get; set; }

    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    public double Contrast { get; set; }

    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    public double Saturation { get; set; }

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    public double Red { get; set; }

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    public double Green { get; set; }

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    public double Blue { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MixEffectKeyAdvancedChromaPropertiesUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var keyerId = reader.ReadByte();

        // Read all property values as 16-bit integers and divide by 10 (to match TypeScript implementation scaling)
        var foregroundLevel = reader.ReadUInt16BigEndian() / 10.0;
        var backgroundLevel = reader.ReadUInt16BigEndian() / 10.0;
        var keyEdge = reader.ReadUInt16BigEndian() / 10.0;

        var spillSuppression = reader.ReadUInt16BigEndian() / 10.0;
        var flareSuppression = reader.ReadUInt16BigEndian() / 10.0;

        // Color adjustment values can be negative, so use signed 16-bit (also scaled by 10)
        var brightness = reader.ReadInt16BigEndian() / 10.0;
        var contrast = reader.ReadInt16BigEndian() / 10.0;
        var saturation = reader.ReadUInt16BigEndian() / 10.0;
        var red = reader.ReadInt16BigEndian() / 10.0;
        var green = reader.ReadInt16BigEndian() / 10.0;
        var blue = reader.ReadInt16BigEndian() / 10.0;

        return new MixEffectKeyAdvancedChromaPropertiesUpdateCommand
        {
            MixEffectId = mixEffectId,
            KeyerId = keyerId,
            ForegroundLevel = foregroundLevel,
            BackgroundLevel = backgroundLevel,
            KeyEdge = keyEdge,
            SpillSuppression = spillSuppression,
            FlareSuppression = flareSuppression,
            Brightness = brightness,
            Contrast = contrast,
            Saturation = saturation,
            Red = red,
            Green = green,
            Blue = blue
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
        if (keyer.AdvancedChromaSettings == null)
            keyer.AdvancedChromaSettings = new UpstreamKeyerAdvancedChromaSettings();

        if (keyer.AdvancedChromaSettings.Properties == null)
            keyer.AdvancedChromaSettings.Properties = new UpstreamKeyerAdvancedChromaProperties();

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
