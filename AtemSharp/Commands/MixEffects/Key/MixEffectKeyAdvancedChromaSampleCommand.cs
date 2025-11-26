using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update advanced chroma key sample settings for an upstream keyer
/// </summary>
[Command("CACC")]
[BufferSize(20)]
public partial class MixEffectKeyAdvancedChromaSampleCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(2)] [NoProperty] private readonly byte _keyerId = keyer.Id;

    /// <summary>
    /// Whether the cursor is enabled
    /// </summary>
    [SerializedField(3, 0)] private bool _enableCursor = keyer.AdvancedChromaSettings.Sample.EnableCursor;

    /// <summary>
    /// Whether to show preview
    /// </summary>
    [SerializedField(4, 1)] private bool _preview = keyer.AdvancedChromaSettings.Sample.Preview;

    /// <summary>
    /// Cursor X position
    /// </summary>
    [SerializedField(6, 2)] [SerializedType(typeof(short))] [ScalingFactor(1000)]
    private double _cursorX = keyer.AdvancedChromaSettings.Sample.CursorX;

    /// <summary>
    /// Cursor Y position
    /// </summary>
    [SerializedField(8, 3)] [SerializedType(typeof(short))] [ScalingFactor(1000)]
    private double _cursorY = keyer.AdvancedChromaSettings.Sample.CursorY;

    /// <summary>
    /// Cursor size
    /// </summary>
    [SerializedField(10, 4)] [ScalingFactor(100)]
    private double _cursorSize = keyer.AdvancedChromaSettings.Sample.CursorSize;

    /// <summary>
    /// Sampled Y (luminance) value
    /// </summary>
    [SerializedField(12, 5)] [ScalingFactor(10000)]
    private double _sampledY = keyer.AdvancedChromaSettings.Sample.SampledY;

    /// <summary>
    /// Sampled Cb (blue-difference chroma) value
    /// </summary>
    [SerializedField(14, 6)] [SerializedType(typeof(short))] [ScalingFactor(10000)]
    private double _sampledCb = keyer.AdvancedChromaSettings.Sample.SampledCb;

    /// <summary>
    /// Sampled Cr (red-difference chroma) value
    /// </summary>
    [SerializedField(16, 7)] [SerializedType(typeof(short))] [ScalingFactor(10000)]
    private double _sampledCr = keyer.AdvancedChromaSettings.Sample.SampledCr;
}
