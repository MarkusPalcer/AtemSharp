using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("KACk")]
internal partial class MixEffectKeyAdvancedChromaPropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;
    [DeserializedField(1)] private byte _keyerId;

    [DeserializedField(2)] [ScalingFactor(10)]
    private double _foregroundLevel;

    [DeserializedField(4)] [ScalingFactor(10)]
    private double _backgroundLevel;

    [DeserializedField(6)] [ScalingFactor(10)]
    private double _keyEdge;

    [DeserializedField(8)] [ScalingFactor(10)]
    private double _spillSuppression;

    [DeserializedField(10)] [ScalingFactor(10)]
    private double _flareSuppression;

    [DeserializedField(12)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _brightness;

    [DeserializedField(14)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _contrast;

    [DeserializedField(16)] [ScalingFactor(10)]
    private double _saturation;

    [DeserializedField(18)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _red;

    [DeserializedField(20)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _green;

    [DeserializedField(22)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _blue;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectId];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);

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
