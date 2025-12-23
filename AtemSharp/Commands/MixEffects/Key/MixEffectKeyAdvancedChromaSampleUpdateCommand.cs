using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("KACC")]
internal partial class MixEffectKeyAdvancedChromaSampleUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _keyerId;

    [DeserializedField(2)] private bool _enableCursor;

    [DeserializedField(3)] private bool _preview;

    [DeserializedField(4)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _cursorX;

    [DeserializedField(6)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _cursorY;

    [DeserializedField(8)] [ScalingFactor(100)]
    private double _cursorSize;

    [DeserializedField(10)] [ScalingFactor(10000)]
    private double _sampledY;

    [DeserializedField(12)] [ScalingFactor(10000)] [SerializedType(typeof(short))]
    private double _sampledCb;

    [DeserializedField(14)] [ScalingFactor(10000)] [SerializedType(typeof(short))]
    private double _sampledCr;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectId];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);

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
