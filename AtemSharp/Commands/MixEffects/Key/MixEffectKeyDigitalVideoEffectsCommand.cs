using AtemSharp.Lib;
using AtemSharp.State.Border;
using AtemSharp.State.Video.MixEffect.UpstreamKeyer;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update DVE settings for an upstream keyer
/// </summary>
[Command("CKDV")]
[BufferSize(64)]
public partial class MixEffectKeyDigitalVideoEffectsCommand(UpstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(4)]
    [NoProperty]
    private readonly byte _mixEffectId = keyer.MixEffectId;

    [SerializedField(5)]
    [NoProperty]
    private readonly byte _keyerId = keyer.Id;

    [SerializedField(8, 0)]
    [SerializedType(typeof(ulong))]
    [ScalingFactor(1000.0)]
    private double _sizeX = keyer.DigitalVideoEffectsSettings.SizeX;

    [SerializedField(12, 1)]
    [SerializedType(typeof(ulong))]
    [ScalingFactor(1000.0)]
    private double _sizeY = keyer.DigitalVideoEffectsSettings.SizeY;

    [SerializedField(16,2)]
    [SerializedType(typeof(long))]
    [ScalingFactor(1000.0)]
    private double _positionX = keyer.DigitalVideoEffectsSettings.PositionX;

    [SerializedField(20,3)]
    [SerializedType(typeof(long))]
    [ScalingFactor(1000.0)]
    private double _positionY = keyer.DigitalVideoEffectsSettings.PositionY;

    [SerializedField(24, 4)]
    [SerializedType(typeof(long))]
    [ScalingFactor(10.0)]
    private double _rotation = keyer.DigitalVideoEffectsSettings.Rotation;

    [SerializedField(28, 5)]
    private bool _borderEnabled = keyer.DigitalVideoEffectsSettings.BorderEnabled;

    [SerializedField(29, 6)]
    private bool _shadowEnabled = keyer.DigitalVideoEffectsSettings.ShadowEnabled;

    [SerializedField(30, 7)]
    private BorderBevel _borderBevel = keyer.DigitalVideoEffectsSettings.BorderBevel;

    [SerializedField(32, 8)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(65536.0 / 16.0)]
    private double _borderOuterWidth = keyer.DigitalVideoEffectsSettings.BorderOuterWidth;

    [SerializedField(34, 9)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(65536.0 / 16.0)]
    private double _borderInnerWidth = keyer.DigitalVideoEffectsSettings.BorderInnerWidth;

    [SerializedField(36, 10)]
    [SerializedType(typeof(byte))]
    private double _borderOuterSoftness = keyer.DigitalVideoEffectsSettings.BorderOuterSoftness;

    [SerializedField(37, 11)]
    [SerializedType(typeof(byte))]
    private double _borderInnerSoftness = keyer.DigitalVideoEffectsSettings.BorderInnerSoftness;

    [SerializedField(38, 12)]
    [SerializedType(typeof(byte))]
    private double _borderBevelSoftness = keyer.DigitalVideoEffectsSettings.BorderBevelSoftness;

    [SerializedField(39, 13)]
    [SerializedType(typeof(byte))]
    private double _borderBevelPosition = keyer.DigitalVideoEffectsSettings.BorderBevelPosition;

    [SerializedField(40, 14)]
    [SerializedType(typeof(byte))]
    private double _borderOpacity = keyer.DigitalVideoEffectsSettings.BorderOpacity;

    [SerializedField(42, 15)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(10.0)]
    private double _borderHue = keyer.DigitalVideoEffectsSettings.BorderHue;

    [SerializedField(44, 16)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(10.0)]
    private double _borderSaturation = keyer.DigitalVideoEffectsSettings.BorderSaturation;

    [SerializedField(46, 17)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(10.0)]
    private double _borderLuma = keyer.DigitalVideoEffectsSettings.BorderLuma;

    [SerializedField(48, 18)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(10.0)]
    private double _lightSourceDirection = keyer.DigitalVideoEffectsSettings.LightSourceDirection;

    [SerializedField(50, 19)]
    [SerializedType(typeof(byte))]
    private double _lightSourceAltitude = keyer.DigitalVideoEffectsSettings.LightSourceAltitude;

    [SerializedField(51, 20)]
    private bool _maskEnabled = keyer.DigitalVideoEffectsSettings.MaskEnabled;

    [SerializedField(52, 21)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(65536.0 * 1.515 / 100.0)]
    private double _maskTop = keyer.DigitalVideoEffectsSettings.MaskTop;

    [SerializedField(54, 22)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(65536.0 * 1.515 / 100.0)]
    private double _maskBottom = keyer.DigitalVideoEffectsSettings.MaskBottom;

    [SerializedField(56, 23)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(65536.0 * 1.515 / 100.0)]
    private double _maskLeft = keyer.DigitalVideoEffectsSettings.MaskLeft;

    [SerializedField(58, 24)]
    [SerializedType(typeof(ushort))]
    [ScalingFactor(65536.0 * 1.515 / 100.0)]
    private double _maskRight = keyer.DigitalVideoEffectsSettings.MaskRight;

    [SerializedField(60, 25)]
    private byte _rate = keyer.DigitalVideoEffectsSettings.Rate;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt32BigEndian(_mixEffectId, 0);
    }
}
