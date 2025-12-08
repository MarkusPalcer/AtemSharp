using System.Drawing;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;
using AtemSharp.Types.Border;

namespace AtemSharp.Commands.MixEffects.Key;

// TODO #80: Capture test data and use test class base
/// <summary>
/// Command to update DVE settings for an upstream keyer
/// </summary>
[Command("CKDV")]
[BufferSize(64)]
public partial class MixEffectKeyDigitalVideoEffectsCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(4)] [NoProperty] private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(5)] [NoProperty] private readonly byte _keyerId = keyer.Id;

    [SerializedField(8, 0)] [SerializedType(typeof(ulong))] [ScalingFactor(1000.0)]
    private double _sizeX = keyer.DigitalVideoEffectsSettings.SizeX;

    [SerializedField(12, 1)] [SerializedType(typeof(ulong))] [ScalingFactor(1000.0)]
    private double _sizeY = keyer.DigitalVideoEffectsSettings.SizeY;

    [SerializedField(16, 2)] [SerializedType(typeof(long))] [ScalingFactor(1000.0)]
    private double _positionX = keyer.DigitalVideoEffectsSettings.Location.X;

    [SerializedField(20, 3)] [SerializedType(typeof(long))] [ScalingFactor(1000.0)]
    private double _positionY = keyer.DigitalVideoEffectsSettings.Location.Y;

    [SerializedField(24, 4)] [SerializedType(typeof(long))] [ScalingFactor(10.0)]
    private double _rotation = keyer.DigitalVideoEffectsSettings.Rotation;

    [SerializedField(28, 5)] private bool _borderEnabled = keyer.DigitalVideoEffectsSettings.Border.Enabled;

    [SerializedField(29, 6)] private bool _shadowEnabled = keyer.DigitalVideoEffectsSettings.ShadowEnabled;

    [SerializedField(30, 7)] private BorderBevel _borderBevel = keyer.DigitalVideoEffectsSettings.Border.Bevel;

    [SerializedField(32, 8)] [SerializedType(typeof(ushort))] [ScalingFactor(65536.0 / 16.0)]
    private double _borderOuterWidth = keyer.DigitalVideoEffectsSettings.Border.OuterWidth;

    [SerializedField(34, 9)] [SerializedType(typeof(ushort))] [ScalingFactor(65536.0 / 16.0)]
    private double _borderInnerWidth = keyer.DigitalVideoEffectsSettings.Border.InnerWidth;

    [SerializedField(36, 10)]
    private byte _borderOuterSoftness = keyer.DigitalVideoEffectsSettings.Border.OuterSoftness;

    [SerializedField(37, 11)]
    private byte _borderInnerSoftness = keyer.DigitalVideoEffectsSettings.Border.InnerSoftness;

    [SerializedField(38, 12)]
    private byte _borderBevelSoftness = keyer.DigitalVideoEffectsSettings.Border.BevelSoftness;

    [SerializedField(39, 13)]
    private byte _borderBevelPosition = keyer.DigitalVideoEffectsSettings.Border.BevelPosition;

    [SerializedField(40, 14)]
    private byte _borderOpacity = keyer.DigitalVideoEffectsSettings.Border.Opacity;

    [SerializedField(42, 15)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderHue = keyer.DigitalVideoEffectsSettings.Border.Color.Hue;

    [SerializedField(44, 16)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderSaturation = keyer.DigitalVideoEffectsSettings.Border.Color.Saturation;

    [SerializedField(46, 17)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderLuma = keyer.DigitalVideoEffectsSettings.Border.Color.Luma;

    [SerializedField(48, 18)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _lightSourceDirection = keyer.DigitalVideoEffectsSettings.Border.LightSourceDirection;

    [SerializedField(50, 19)] [SerializedType(typeof(byte))]
    private double _lightSourceAltitude = keyer.DigitalVideoEffectsSettings.Border.LightSourceAltitude;

    [SerializedField(51, 20)] private bool _maskEnabled = keyer.DigitalVideoEffectsSettings.MaskEnabled;

    [SerializedField(52, 21)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskTop = keyer.DigitalVideoEffectsSettings.MaskTop;

    [SerializedField(54, 22)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskBottom = keyer.DigitalVideoEffectsSettings.MaskBottom;

    [SerializedField(56, 23)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskLeft = keyer.DigitalVideoEffectsSettings.MaskLeft;

    [SerializedField(58, 24)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _maskRight = keyer.DigitalVideoEffectsSettings.MaskRight;

    [SerializedField(60, 25)] private byte _rate = keyer.DigitalVideoEffectsSettings.Rate;

    public PointF Location
    {
        get => new((float)_positionX, (float)_positionY);
        set
        {
            PositionX = value.X;
            PositionY = value.Y;
        }
    }

    private void SerializeInternal(byte[] buffer)
    {
        // TODO #79: Switch to generated serialization
        buffer.WriteUInt32BigEndian(Flag, 0);
    }
}
