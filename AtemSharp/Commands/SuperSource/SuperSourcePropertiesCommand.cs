using AtemSharp.State.Border;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Commands.SuperSource;

[Command("CSSc")]
[BufferSize(36)]
public partial class SuperSourcePropertiesCommand(State.Video.SuperSource.SuperSource superSource) : SerializedCommand
{
    [SerializedField(4, 0)] private ushort _artFillSource = superSource.FillSource;
    [SerializedField(6, 1)] private ushort _artCutSource = superSource.CutSource;
    [SerializedField(8, 2)] private ArtOption _artOption = superSource.Option;
    [SerializedField(9, 3)] private bool _artPremultiplied = superSource.PreMultiplied;

    [SerializedField(10, 4)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artClip = superSource.Clip;

    [SerializedField(12, 5)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artGain = superSource.Gain;

    [SerializedField(14, 6)] private bool _artInvertKey = superSource.InvertKey;
    [SerializedField(15, 7)] private bool _enabled = superSource.Border.Enabled;
    [SerializedField(16, 8)] private BorderBevel _bevel = superSource.Border.Bevel;

    [SerializedField(18, 9)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _outerWidth = superSource.Border.OuterWidth;

    [SerializedField(20, 10)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _innerWidth = superSource.Border.InnerWidth;

    [SerializedField(22, 11)] private byte _outerSoftness = superSource.Border.OuterSoftness;
    [SerializedField(23, 12)] private byte _innerSoftness = superSource.Border.InnerSoftness;
    [SerializedField(24, 13)] private byte _bevelSoftness = superSource.Border.BevelSoftness;
    [SerializedField(25, 14)] private byte _bevelPosition = superSource.Border.BevelPosition;

    [SerializedField(26, 15)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _hue = superSource.Border.Hue;

    [SerializedField(28, 16)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _saturation = superSource.Border.Saturation;

    [SerializedField(30, 17)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _luma = superSource.Border.Luma;

    [SerializedField(32, 18)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _lightSourceDirection = superSource.Border.LightSourceDirection;

    [SerializedField(34, 19)] [SerializedType(typeof(byte))]
    private double _lightSourceAltitude = superSource.Border.LightSourceAltitude;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt32BigEndian(Flag, 0);
    }
}
