using AtemSharp.State;
using AtemSharp.State.Border;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.SuperSource;

[Command("SSBd", ProtocolVersion.V8_0)]
public partial class SuperSourceBorderUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _superSourceId;
    [DeserializedField(1)] private bool _borderEnabled;
    [DeserializedField(2)] private BorderBevel _borderBevel;

    [DeserializedField(4)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _outerWidth;

    [DeserializedField(6)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _innerWidth;

    [DeserializedField(8)] private byte _outerSoftness;
    [DeserializedField(9)] private byte _innerSoftness;
    [DeserializedField(10)] private byte _bevelSoftness;
    [DeserializedField(11)] private byte _bevelPosition;

    [DeserializedField(12)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _hue;

    [DeserializedField(14)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _saturation;

    [DeserializedField(16)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _luma;

    [DeserializedField(18)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _lightSourceDirection;

    [DeserializedField(20)] [SerializedType(typeof(byte))]
    private double _lightSourceAltitude;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var superSource = state.Video.SuperSources[_superSourceId];
        superSource.Border.Enabled = _borderEnabled;
        superSource.Border.Bevel = _borderBevel;
        superSource.Border.OuterWidth = _outerWidth;
        superSource.Border.InnerWidth = _innerWidth;
        superSource.Border.OuterSoftness = _outerSoftness;
        superSource.Border.InnerSoftness = _innerSoftness;
        superSource.Border.BevelSoftness = _bevelSoftness;
        superSource.Border.BevelPosition = _bevelPosition;
        superSource.Border.Hue = _hue;
        superSource.Border.Saturation = _saturation;
        superSource.Border.Luma = _luma;
        superSource.Border.LightSourceDirection = _lightSourceDirection;
        superSource.Border.LightSourceAltitude = _lightSourceAltitude;
    }
}
