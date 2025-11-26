using AtemSharp.State.Border;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.SuperSource;

[Command("CSBd", ProtocolVersion.V8_0)]
[BufferSize(24)]
public partial class SuperSourceBorderCommand(State.Video.SuperSource.SuperSource superSource) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _superSourceId = superSource.Id;
    [SerializedField(3, 0)] private bool _enabled = superSource.Border.Enabled;
    [SerializedField(4, 1)] private BorderBevel _bevel = superSource.Border.Bevel;

    [SerializedField(6, 2)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _outerWidth = superSource.Border.OuterWidth;

    [SerializedField(8, 3)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _innerWidth = superSource.Border.InnerWidth;

    [SerializedField(10, 4)] private byte _outerSoftness = superSource.Border.OuterSoftness;
    [SerializedField(11, 5)] private byte _innerSoftness = superSource.Border.InnerSoftness;
    [SerializedField(12, 6)] private byte _bevelSoftness = superSource.Border.BevelSoftness;
    [SerializedField(13, 7)] private byte _bevelPosition = superSource.Border.BevelPosition;

    [SerializedField(14, 8)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _hue = superSource.Border.Hue;

    [SerializedField(16, 9)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _saturation = superSource.Border.Saturation;

    [SerializedField(18, 10)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _luma = superSource.Border.Luma;

    [SerializedField(20, 11)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _lightSourceDirection = superSource.Border.LightSourceDirection;

    [SerializedField(22, 12)] [SerializedType(typeof(byte))]
    private double _lightSourceAltitude = superSource.Border.LightSourceAltitude;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
