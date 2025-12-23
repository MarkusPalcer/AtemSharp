using System.Drawing;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Used to set the properties of a keyframe for the UpstreamKeyer fly animation
/// </summary>
// TODO #82: Capture real test data and verify implementation and tests
[Command("CKFP")]
[BufferSize(56)]
public partial class MixEffectKeyFlyKeyframeCommand(UpstreamKeyerFlyKeyframe keyframe) : SerializedCommand
{
    [SerializedField(4)] [NoProperty] private readonly byte _mixEffectId = keyframe.MixEffectId;
    [SerializedField(5)] [NoProperty] private readonly byte _upstreamKeyerId = keyframe.UpstreamKeyerId;
    [SerializedField(6)] [NoProperty] private readonly byte _keyframeId = keyframe.Id;

    [SerializedField(8, 0)] [ScalingFactor(1000)] [SerializedType(typeof(uint))]
    private double _sizeX = keyframe.Size.Width;

    [SerializedField(12, 1)] [ScalingFactor(1000)] [SerializedType(typeof(uint))]
    private double _sizeY = keyframe.Size.Height;

    [SerializedField(16, 2)] [ScalingFactor(1000)] [SerializedType(typeof(int))]
    private double _positionX = keyframe.Location.X;

    [SerializedField(20, 3)] [ScalingFactor(1000)] [SerializedType(typeof(int))]
    private double _positionY = keyframe.Location.Y;

    [SerializedField(24, 4)] [ScalingFactor(10)] [SerializedType(typeof(int))]
    private double _rotation = keyframe.Rotation;

    [SerializedField(28, 5)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _borderOuterWidth = keyframe.Border.OuterWidth;

    [SerializedField(30, 6)] [ScalingFactor(100)] [SerializedType(typeof(ushort))]
    private double _borderInnerWidth = keyframe.Border.InnerWidth;

    [SerializedField(32, 7)] private byte _borderOuterSoftness = keyframe.Border.OuterSoftness;
    [SerializedField(33, 8)] private byte _borderInnerSoftness = keyframe.Border.InnerSoftness;
    [SerializedField(34, 9)] private byte _borderBevelSoftness = keyframe.Border.BevelSoftness;
    [SerializedField(35, 10)] private byte _borderBevelPosition = keyframe.Border.BevelPosition;

    [SerializedField(36, 11)] private byte _borderOpacity = keyframe.Border.Opacity;

    [SerializedField(38, 12)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _borderHue = keyframe.Border.Color.Hue;

    [SerializedField(40, 13)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _borderSaturation = keyframe.Border.Color.Saturation;

    [SerializedField(42, 14)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _borderLuma = keyframe.Border.Color.Luma;

    [SerializedField(44, 15)] [ScalingFactor(10)] [SerializedType(typeof(ushort))]
    private double _lightSourceDirection = keyframe.Shadow.LightSourceDirection;

    [SerializedField(46, 16)] private byte _lightSourceAltitude = (byte)keyframe.Shadow.LightSourceAltitude;

    //[SerializedField(47)] private bool _maskEnabled = keyframe.Mask.Enabled;

    [SerializedField(48, 17)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskTop = keyframe.Mask.Top;

    [SerializedField(50, 18)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskBottom = keyframe.Mask.Bottom;

    [SerializedField(52, 19)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskLeft = keyframe.Mask.Left;

    [SerializedField(54, 20)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _maskRight = keyframe.Mask.Right;

    public PointF Location
    {
        get => new((float)_positionX, (float)_positionY);
        set
        {
            PositionX = value.X;
            PositionY = value.Y;
        }
    }

    public SizeF Size
    {
        get => new((float)_sizeX, (float)_sizeY);
        set
        {
            SizeX =  value.Width;
            SizeY =  value.Height;
        }
    }

    public RectangleF Bounds
    {
        get => new(Location, Size);
        set
        {
            Location = value.Location;
            Size = value.Size;
        }
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt32BigEndian(Flag, 0);
    }
}
