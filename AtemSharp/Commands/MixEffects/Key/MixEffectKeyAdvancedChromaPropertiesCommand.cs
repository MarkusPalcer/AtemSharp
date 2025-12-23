using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Used to update advanced chroma key properties for an upstream keyer
/// </summary>
[Command("CACK")]
[BufferSize(28)]
public partial class MixEffectKeyAdvancedChromaPropertiesCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(3)] [NoProperty] private readonly byte _keyerId = keyer.Id;

    [SerializedField(4, 0)] [ScalingFactor(10)]
    private double _foregroundLevel = keyer.AdvancedChromaSettings.Properties.ForegroundLevel;

    [SerializedField(6, 1)] [ScalingFactor(10)]
    private double _backgroundLevel = keyer.AdvancedChromaSettings.Properties.BackgroundLevel;

    [SerializedField(8, 2)] [ScalingFactor(10)]
    private double _keyEdge = keyer.AdvancedChromaSettings.Properties.KeyEdge;

    [SerializedField(10, 3)] [ScalingFactor(10)]
    private double _spillSuppression = keyer.AdvancedChromaSettings.Properties.SpillSuppression;

    [SerializedField(12, 4)] [ScalingFactor(10)]
    private double _flareSuppression = keyer.AdvancedChromaSettings.Properties.FlareSuppression;

    [SerializedField(14, 5)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _brightness = keyer.AdvancedChromaSettings.Properties.Brightness;

    [SerializedField(16, 6)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _contrast = keyer.AdvancedChromaSettings.Properties.Contrast;

    [SerializedField(18, 7)] [ScalingFactor(10)]
    private double _saturation = keyer.AdvancedChromaSettings.Properties.Saturation;

    [SerializedField(20, 8)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _red = keyer.AdvancedChromaSettings.Properties.Red;

    [SerializedField(22, 9)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _green = keyer.AdvancedChromaSettings.Properties.Green;

    [SerializedField(24, 10)] [ScalingFactor(10)] [SerializedType(typeof(short))]
    private double _blue = keyer.AdvancedChromaSettings.Properties.Blue;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
