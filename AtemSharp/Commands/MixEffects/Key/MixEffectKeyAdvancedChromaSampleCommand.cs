using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update advanced chroma key sample settings for an upstream keyer
/// </summary>
[Command("CACC")]
[BufferSize(20)]
public partial class MixEffectKeyAdvancedChromaSampleCommand : SerializedCommand
{
    [SerializedField(1)]
    [NoProperty]
    private readonly byte _mixEffectId;

    [SerializedField(2)]
    [NoProperty]
    private readonly byte _keyerId;

    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    [SerializedField(3, 0)]
    private bool _enableCursor;

    /// <summary>
    /// Whether to show preview
    /// </summary>
    [SerializedField(4, 1)]
    private bool _preview;

    /// <summary>
    /// Cursor X position
    /// </summary>
    [SerializedField(6, 2)]
    [SerializedType(typeof(short))]
    [ScalingFactor(1000)]
    private double _cursorX;

    /// <summary>
    /// Cursor Y position
    /// </summary>
    [SerializedField(8, 3)]
    [SerializedType(typeof(short))]
    [ScalingFactor(1000)]
    private double _cursorY;

    /// <summary>
    /// Cursor size
    /// </summary>
    [SerializedField(10, 4)]
    [ScalingFactor(100)]
    private double _cursorSize;

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    [SerializedField(12, 5)]
    [ScalingFactor(10000)]
    private double _sampledY;

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    [SerializedField(14, 6)]
    [SerializedType(typeof(short))]
    [ScalingFactor(10000)]
    private double _sampledCb;

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    [SerializedField(16, 7)]
    [SerializedType(typeof(short))]
    [ScalingFactor(10000)]
    private double _sampledCr;

    public MixEffectKeyAdvancedChromaSampleCommand(UpstreamKeyer keyer)
    {
        _mixEffectId = keyer.MixEffectId;
        _keyerId = keyer.Id;

        var sample = keyer.AdvancedChromaSettings.Sample;

        // Initialize from current state (direct field access = no flags set)
        _enableCursor = sample.EnableCursor;
        _preview = sample.Preview;
        _cursorX = sample.CursorX;
        _cursorY = sample.CursorY;
        _cursorSize = sample.CursorSize;
        _sampledY = sample.SampledY;
        _sampledCb = sample.SampledCb;
        _sampledCr = sample.SampledCr;
    }
}
