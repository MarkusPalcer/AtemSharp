using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update advanced chroma key properties for an upstream keyer
/// </summary>
[Command("CACK")]
[BufferSize(28)]
public partial class MixEffectKeyAdvancedChromaPropertiesCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(3)] [NoProperty] private readonly byte _keyerId = keyer.Id;

    /// <summary>
    /// Foreground level value
    /// </summary>
    [SerializedField(4, 0)] [ScalingFactor(10)]
    private double _foregroundLevel = keyer.AdvancedChromaSettings.Properties.ForegroundLevel;

    /// <summary>
    /// Background level value
    /// </summary>
    [SerializedField(6, 1)] [ScalingFactor(10)]
    private double _backgroundLevel = keyer.AdvancedChromaSettings.Properties.BackgroundLevel;

    /// <summary>
    /// Key edge value
    /// </summary>
    [SerializedField(8, 2)] [ScalingFactor(10)]
    private double _keyEdge = keyer.AdvancedChromaSettings.Properties.KeyEdge;

    /// <summary>
    /// Spill suppression value
    /// </summary>
    [SerializedField(10, 3)] [ScalingFactor(10)]
    private double _spillSuppression = keyer.AdvancedChromaSettings.Properties.SpillSuppression;

    /// <summary>
    /// Flare suppression value
    /// </summary>
    [SerializedField(12, 4)] [ScalingFactor(10)]
    private double _flareSuppression = keyer.AdvancedChromaSettings.Properties.FlareSuppression;

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    [SerializedField(14, 5)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _brightness = keyer.AdvancedChromaSettings.Properties.Brightness;


    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    [SerializedField(16, 6)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _contrast = keyer.AdvancedChromaSettings.Properties.Contrast;


    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    [SerializedField(18, 7)] [ScalingFactor(10)]
    private double _saturation = keyer.AdvancedChromaSettings.Properties.Saturation;

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    [SerializedField(20, 8)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _red = keyer.AdvancedChromaSettings.Properties.Red;

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    [SerializedField(22, 9)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _green = keyer.AdvancedChromaSettings.Properties.Green;

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    [SerializedField(24, 10)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _blue = keyer.AdvancedChromaSettings.Properties.Blue;


    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
