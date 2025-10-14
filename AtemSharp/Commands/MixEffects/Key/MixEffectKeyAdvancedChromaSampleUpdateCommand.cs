using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public int MixEffectId { get; init; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerId { get; init; }

    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    public bool EnableCursor { get; init; }

    /// <summary>
    /// Whether to show preview
    /// </summary>
    public bool Preview { get; init; }

    /// <summary>
    /// Cursor X position
    /// </summary>
    public double CursorX { get; init; }

    /// <summary>
    /// Cursor Y position
    /// </summary>
    public double CursorY { get; init; }

    /// <summary>
    /// Cursor size
    /// </summary>
    public double CursorSize { get; init; }

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    public double SampledY { get; init; }

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    public double SampledCb { get; init; }

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    public double SampledCr { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MixEffectKeyAdvancedChromaSampleUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MixEffectKeyAdvancedChromaSampleUpdateCommand
        {
            MixEffectId = rawCommand.ReadUInt8(0),
            KeyerId = rawCommand.ReadUInt8(1),
            EnableCursor = rawCommand.ReadBoolean(2),
            Preview = rawCommand.ReadBoolean(3),
            CursorX = rawCommand.ReadInt16BigEndian(4) / 1000.0,
            CursorY = rawCommand.ReadInt16BigEndian(6) / 1000.0,
            CursorSize = rawCommand.ReadUInt16BigEndian(8) / 100.0,
            SampledY = rawCommand.ReadUInt16BigEndian(10) / 10000.0,
            SampledCb = rawCommand.ReadInt16BigEndian(12) / 10000.0,
            SampledCr = rawCommand.ReadInt16BigEndian(14) / 10000.0
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

        keyer.AdvancedChromaSettings.Sample ??= new UpstreamKeyerAdvancedChromaSample();

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
