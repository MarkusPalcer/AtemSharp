using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer advanced chroma sample settings update
/// </summary>
[Command("KACC")]
public partial class MixEffectKeyAdvancedChromaSampleUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    [DeserializedField(0)]
    private byte _mixEffectId;

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    [DeserializedField(1)]
    private byte _keyerId;

    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    [DeserializedField(2)]
    private bool _enableCursor;

    /// <summary>
    /// Whether to show preview
    /// </summary>
    [DeserializedField(3)]
    private bool _preview;

    /// <summary>
    /// Cursor X position
    /// </summary>
    [DeserializedField(4)]
    [ScalingFactor(1000)]
    [SerializedType(typeof(short))]
    private double _cursorX;

    /// <summary>
    /// Cursor Y position
    /// </summary>
    [DeserializedField(6)]
    [ScalingFactor(1000)]
    [SerializedType(typeof(short))]
    private double _cursorY;

    /// <summary>
    /// Cursor size
    /// </summary>
    [DeserializedField(8)]
    [ScalingFactor(100)]
    private double _cursorSize;

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    [DeserializedField(10)]
    [ScalingFactor(10000)]
    private double _sampledY;

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    [DeserializedField(12)]
    [ScalingFactor(10000)]
    [SerializedType(typeof(short))]
    private double _sampledCb;

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    [DeserializedField(14)]
    [ScalingFactor(10000)]
    [SerializedType(typeof(short))]
    private double _sampledCr;

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
        keyer.Id = KeyerId;

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
