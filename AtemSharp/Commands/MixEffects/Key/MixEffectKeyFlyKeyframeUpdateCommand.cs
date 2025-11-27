using System.Drawing;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

[Command("KKFP")]
public partial class MixEffectKeyFlyKeyframeUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;
    [DeserializedField(1)] private byte _upstreamKeyerId;
    [DeserializedField(2)] private byte _keyframeId;

    [DeserializedField(4)] [ScalingFactor(1000)] [SerializedType(typeof(uint))]
    private double _sizeX;

    [DeserializedField(8)] [ScalingFactor(1000)] [SerializedType(typeof(uint))]
    private double _sizeY;

    [DeserializedField(12)] [ScalingFactor(1000)] [SerializedType(typeof(int))]
    private double _positionX;

    [DeserializedField(16)] [ScalingFactor(1000)] [SerializedType(typeof(int))]
    private double _positionY;

    [DeserializedField(20)] [ScalingFactor(10)] [SerializedType(typeof(int))]
    private double _rotation;

    [DeserializedField(24)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _borderOuterWidth;

    [DeserializedField(26)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _borderInnerWidth;

    [DeserializedField(28)] private byte _borderOuterSoftness;
    [DeserializedField(29)] private byte _borderInnerSoftness;
    [DeserializedField(30)] private byte _borderBevelSoftness;
    [DeserializedField(31)] private byte _borderBevelPosition;

    [DeserializedField(32)] private byte _borderOpacity;

    [DeserializedField(34)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _borderHue;

    [DeserializedField(36)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _borderSaturation;

    [DeserializedField(38)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _borderLuma;

    [DeserializedField(40)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _lightSourceDirection;

    [DeserializedField(42)] private byte _lightSourceAltitude;

    //[DeserializedField(43)] private bool _maskEnabled;

    [DeserializedField(44)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskTop;

    [DeserializedField(46)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskBottom;

    [DeserializedField(48)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskLeft;

    [DeserializedField(50)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskRight;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var keyframe = state.Video.MixEffects[_mixEffectId].UpstreamKeyers[_upstreamKeyerId].Keyframes[_keyframeId - 1];
        keyframe.Size = new SizeF((float)_sizeX, (float)_sizeY);
        keyframe.Location = new PointF((float)_positionX, (float)_positionY);
        keyframe.Rotation = _rotation;
        keyframe.Border.OuterWidth = _borderOuterWidth;
        keyframe.Border.InnerWidth = _borderInnerWidth;
        keyframe.Border.OuterSoftness = _borderOuterSoftness;
        keyframe.Border.InnerSoftness = _borderInnerSoftness;
        keyframe.Border.BevelSoftness = _borderBevelSoftness;
        keyframe.Border.BevelPosition = _borderBevelPosition;
        keyframe.Border.Opacity = _borderOpacity;
        keyframe.Border.Hue = _borderHue;
        keyframe.Border.Saturation = _borderSaturation;
        keyframe.Border.Luma = _borderLuma;
        keyframe.LightSourceDirection = _lightSourceDirection;
        keyframe.LightSourceAltitude = _lightSourceAltitude;
        // keyframe.Mask.Enabled = _maskEnabled;
        keyframe.Mask.Top = _maskTop;
        keyframe.Mask.Bottom = _maskBottom;
        keyframe.Mask.Left = _maskLeft;
        keyframe.Mask.Right = _maskRight;
    }
}
