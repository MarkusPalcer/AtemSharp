using AtemSharp.State;
using AtemSharp.State.Video.SuperSource;
using AtemSharp.Types;
using AtemSharp.Types.Border;

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

    [DeserializedField(14)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _outerWidth;

    [DeserializedField(16)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _innerWidth;

    [DeserializedField(18)] private byte _outerSoftness;
    [DeserializedField(19)] private byte _innerSoftness;
    [DeserializedField(20)] private byte _bevelSoftness;
    [DeserializedField(21)] private byte _bevelPosition;

    [DeserializedField(22)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _hue;

    [DeserializedField(24)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _saturation;

    [DeserializedField(26)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _luma;

    [DeserializedField(28)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _lightSourceDirection;

    [DeserializedField(30)] [SerializedType(typeof(byte))]
    private double _lightSourceAltitude;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var superSource = state.Video.SuperSources[0];
        superSource.Border.Enabled = _borderEnabled;
        superSource.Border.Bevel = _borderBevel;
        superSource.Border.OuterWidth = _outerWidth;
        superSource.Border.InnerWidth = _innerWidth;
        superSource.Border.OuterSoftness = _outerSoftness;
        superSource.Border.InnerSoftness = _innerSoftness;
        superSource.Border.BevelSoftness = _bevelSoftness;
        superSource.Border.BevelPosition = _bevelPosition;
        superSource.Border.Color = new HslColor(_hue, _saturation, _luma);
        superSource.Shadow.LightSourceDirection = _lightSourceDirection;
        superSource.Shadow.LightSourceAltitude = _lightSourceAltitude;
        superSource.FillSource = _artFillSource;
        superSource.CutSource = _artCutSource;
        superSource.Option = _artOption;
        superSource.PreMultiplied = _artPremultiplied;
        superSource.Clip = _artClip;
        superSource.Gain = _artGain;
        superSource.InvertKey = _artInvertKey;
    }
}
