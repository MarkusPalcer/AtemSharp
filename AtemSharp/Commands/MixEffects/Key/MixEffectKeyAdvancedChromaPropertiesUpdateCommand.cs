using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command received from ATEM device containing upstream keyer advanced chroma properties update
/// </summary>
[Command("KACk")]
public partial class MixEffectKeyAdvancedChromaPropertiesUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    [DeserializedField(0)] private byte _mixEffectId;

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    [DeserializedField(1)] private byte _keyerId;

    /// <summary>
    /// Foreground level value
    /// </summary>
    [DeserializedField(2)] [ScalingFactor(10)]
    private double _foregroundLevel;

    /// <summary>
    /// Background level value
    /// </summary>
    [DeserializedField(4)] [ScalingFactor(10)]
    private double _backgroundLevel;

    /// <summary>
    /// Key edge value
    /// </summary>
    [DeserializedField(6)] [ScalingFactor(10)]
    private double _keyEdge;

    /// <summary>
    /// Spill suppression value
    /// </summary>
    [DeserializedField(8)] [ScalingFactor(10)]
    private double _spillSuppression;

    /// <summary>
    /// Flare suppression value
    /// </summary>
    [DeserializedField(10)] [ScalingFactor(10)]
    private double _flareSuppression;

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    [DeserializedField(12)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _brightness;

    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    [DeserializedField(14)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _contrast;

    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    [DeserializedField(16)] [ScalingFactor(10)]
    private double _saturation;

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    [DeserializedField(18)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _red;

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    [DeserializedField(20)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _green;

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    [DeserializedField(22)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _blue;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectId];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);
        keyer.Id = KeyerId;

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
