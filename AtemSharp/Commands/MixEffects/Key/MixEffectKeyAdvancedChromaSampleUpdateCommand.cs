using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer advanced chroma sample settings update
/// </summary>
[Command("KACC")]
public class MixEffectKeyAdvancedChromaSampleUpdateCommand : IDeserializedCommand
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
    /// Whether the cursor is enabled
    /// </summary>
    public bool EnableCursor { get; set; }

    /// <summary>
    /// Whether to show preview
    /// </summary>
    public bool Preview { get; set; }

    /// <summary>
    /// Cursor X position
    /// </summary>
    public double CursorX { get; set; }

    /// <summary>
    /// Cursor Y position
    /// </summary>
    public double CursorY { get; set; }

    /// <summary>
    /// Cursor size
    /// </summary>
    public double CursorSize { get; set; }

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    public double SampledY { get; set; }

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    public double SampledCb { get; set; }

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    public double SampledCr { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MixEffectKeyAdvancedChromaSampleUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var mixEffectId = reader.ReadByte();
        var keyerId = reader.ReadByte();
        var enableCursor = reader.ReadBoolean();
        var preview = reader.ReadBoolean();

        // Read position values and divide by scaling factors (to match TypeScript implementation scaling)
        var cursorX = reader.ReadInt16BigEndian() / 1000.0;
        var cursorY = reader.ReadInt16BigEndian() / 1000.0;

        // Read cursor size and divide by 100
        var cursorSize = reader.ReadUInt16BigEndian() / 100.0;

        // Read sampled values and divide by 10000
        var sampledY = reader.ReadUInt16BigEndian() / 10000.0;
        var sampledCb = reader.ReadInt16BigEndian() / 10000.0;
        var sampledCr = reader.ReadInt16BigEndian() / 10000.0;

        return new MixEffectKeyAdvancedChromaSampleUpdateCommand
        {
            MixEffectId = mixEffectId,
            KeyerId = keyerId,
            EnableCursor = enableCursor,
            Preview = preview,
            CursorX = cursorX,
            CursorY = cursorY,
            CursorSize = cursorSize,
            SampledY = sampledY,
            SampledCb = sampledCb,
            SampledCr = sampledCr
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

        if (keyer.AdvancedChromaSettings.Sample == null)
            keyer.AdvancedChromaSettings.Sample = new UpstreamKeyerAdvancedChromaSample();

        // Update the advanced chroma sample settings
        var sample = keyer.AdvancedChromaSettings.Sample;
        sample.EnableCursor = EnableCursor;
        sample.Preview = Preview;
        sample.CursorX = CursorX;
        sample.CursorY = CursorY;
        sample.CursorSize = CursorSize;
        sample.SampledY = SampledY;
        sample.SampledCb = SampledCb;
        sample.SampledCr = SampledCr;
    }
}
