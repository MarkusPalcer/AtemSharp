using System.Drawing;
using AtemSharp.State;
using AtemSharp.Types;
using AtemSharp.Types.Border;

namespace AtemSharp.Commands.MixEffects.Key;

// TODO #81: Capture test data and create test cases
[Command("KeDV")]
internal partial class MixEffectKeyDigitalVideoEffectsUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _mixEffectId;

    [DeserializedField(1)] private byte _keyerId;

    [DeserializedField(4)] [SerializedType(typeof(uint))] [ScalingFactor(1000.0)]
    private double _sizeX;

    [DeserializedField(8)] [SerializedType(typeof(uint))] [ScalingFactor(1000.0)]
    private double _sizeY;

    [DeserializedField(12)] [SerializedType(typeof(int))] [ScalingFactor(1000.0)]
    private double _positionX;

    [DeserializedField(16)] [SerializedType(typeof(int))] [ScalingFactor(1000.0)]
    private double _positionY;

    [DeserializedField(20)] [SerializedType(typeof(int))] [ScalingFactor(10.0)]
    private double _rotation;

    [DeserializedField(24)] private bool _borderEnabled;

    [DeserializedField(25)] private bool _shadowEnabled;

    [DeserializedField(26)] private BorderBevel _borderBevel;

    [DeserializedField(28)] [SerializedType(typeof(ushort))] [ScalingFactor(100.0)]
    private double _borderOuterWidth;

    [DeserializedField(30)] [SerializedType(typeof(ushort))] [ScalingFactor(100.0)]
    private double _borderInnerWidth;

    [DeserializedField(32)] private byte _borderOuterSoftness;

    [DeserializedField(33)] private byte _borderInnerSoftness;

    [DeserializedField(34)] private byte _borderBevelSoftness;

    [DeserializedField(35)] private byte _borderBevelPosition;

    [DeserializedField(36)] private byte _borderOpacity;

    [DeserializedField(38)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderHue;

    [DeserializedField(40)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderSaturation;

    [DeserializedField(42)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderLuma;

    [DeserializedField(44)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _lightSourceDirection;

    [DeserializedField(46)] [SerializedType(typeof(byte))]
    private double _lightSourceAltitude;

    [DeserializedField(47)] private bool _maskEnabled;

    [DeserializedField(48)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskTop;

    [DeserializedField(50)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskBottom;

    [DeserializedField(52)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskLeft;

    [DeserializedField(54)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskRight;

    [DeserializedField(56)] private byte _rate;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectId];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);

        // Update the DVE settings
        keyer.DigitalVideoEffectsSettings.Size = new SizeF((float)SizeX, (float)SizeY);
        keyer.DigitalVideoEffectsSettings.Location = new PointF((float)PositionX, (float)PositionY);
        keyer.DigitalVideoEffectsSettings.Rotation = Rotation;
        keyer.DigitalVideoEffectsSettings.Border.Enabled = BorderEnabled;
        keyer.DigitalVideoEffectsSettings.Border.Bevel = BorderBevel;
        keyer.DigitalVideoEffectsSettings.Border.OuterWidth = BorderOuterWidth;
        keyer.DigitalVideoEffectsSettings.Border.InnerWidth = BorderInnerWidth;
        keyer.DigitalVideoEffectsSettings.Border.OuterSoftness = BorderOuterSoftness;
        keyer.DigitalVideoEffectsSettings.Border.InnerSoftness = BorderInnerSoftness;
        keyer.DigitalVideoEffectsSettings.Border.BevelSoftness = BorderBevelSoftness;
        keyer.DigitalVideoEffectsSettings.Border.BevelPosition = BorderBevelPosition;
        keyer.DigitalVideoEffectsSettings.Border.Opacity = BorderOpacity;
        keyer.DigitalVideoEffectsSettings.Border.Color = new HslColor(BorderHue, BorderSaturation, BorderLuma);
        keyer.DigitalVideoEffectsSettings.Shadow.Enabled = ShadowEnabled;
        keyer.DigitalVideoEffectsSettings.Shadow.LightSourceDirection = LightSourceDirection;
        keyer.DigitalVideoEffectsSettings.Shadow.LightSourceAltitude = LightSourceAltitude;
        keyer.DigitalVideoEffectsSettings.Mask.Enabled = MaskEnabled;
        keyer.DigitalVideoEffectsSettings.Mask.Top = MaskTop;
        keyer.DigitalVideoEffectsSettings.Mask.Bottom = MaskBottom;
        keyer.DigitalVideoEffectsSettings.Mask.Left = MaskLeft;
        keyer.DigitalVideoEffectsSettings.Mask.Right = MaskRight;
        keyer.DigitalVideoEffectsSettings.Rate = Rate;
    }
}
