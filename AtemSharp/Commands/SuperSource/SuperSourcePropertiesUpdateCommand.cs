using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.SuperSource;

[Command("SSrc")]
public partial class SuperSourcePropertiesUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _artFillSource;
    [DeserializedField(2)] private ushort _artCutSource;
    [DeserializedField(4)] private ArtOption _artOption;
    [DeserializedField(5)] private bool _artPremultiplied;

    [DeserializedField(6)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artClip;

    [DeserializedField(8)] [SerializedType(typeof(ushort))] [ScalingFactor(10)]
    private double _artGain;

    [DeserializedField(10)] private bool _artInvertKey;

    [DeserializedField(11)] private bool _borderEnabled;
    [DeserializedField(12)] private BorderBevel _borderBevel;
    [DeserializedField(14)] [ScalingFactor(100)] [SerializedType(typeof(ushort))] private double _outerWidth;
    [DeserializedField(16)] [ScalingFactor(100)] [SerializedType(typeof(ushort))] private double _innerWidth;
    [DeserializedField(18)] private byte _outerSoftness;
    [DeserializedField(19)] private byte _innerSoftness;
    [DeserializedField(20)] private byte _bevelSoftness;
    [DeserializedField(21)] private byte _bevelPosition;
    [DeserializedField(22)] [ScalingFactor(10)] [SerializedType(typeof(ushort))] private double _hue;
    [DeserializedField(24)] [ScalingFactor(10)] [SerializedType(typeof(ushort))] private double _saturation;
    [DeserializedField(26)] [ScalingFactor(10)] [SerializedType(typeof(ushort))] private double _luma;
    [DeserializedField(28)] [ScalingFactor(10)] [SerializedType(typeof(ushort))] private double _lightSourceDirection;
    [DeserializedField(30)] [SerializedType(typeof(byte))] private double _lightSourceAltitude;

    public void ApplyToState(AtemState state)
    {
        var superSource = state.Video.SuperSources[0];
        superSource.Border = new SuperSourceBorderProperties
        {
            Enabled = _borderEnabled,
            Bevel = _borderBevel,
            OuterWidth = _outerWidth,
            InnerWidth = _innerWidth,
            OuterSoftness = _outerSoftness,
            InnerSoftness = _innerSoftness,
            BevelSoftness = _bevelSoftness,
            BevelPosition = _bevelPosition,
            Hue = _hue,
            Saturation = _saturation,
            Luma = _luma,
            LightSourceDirection = _lightSourceDirection,
            LightSourceAltitude = _lightSourceAltitude,
        };
        superSource.FillSource = _artFillSource;
        superSource.CutSource = _artCutSource;
        superSource.Option = _artOption;
        superSource.PreMultiplied = _artPremultiplied;
        superSource.Clip = _artClip;
        superSource.Gain = _artGain;
        superSource.InvertKey = _artInvertKey;
    }
}
