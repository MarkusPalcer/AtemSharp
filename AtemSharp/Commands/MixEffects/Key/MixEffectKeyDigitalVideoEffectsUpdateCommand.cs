using System.Drawing;
using AtemSharp.State;
using AtemSharp.Types;
using AtemSharp.Types.Border;

namespace AtemSharp.Commands.MixEffects.Key;

// TODO #81: Capture test data and create test cases
/// <summary>
/// Command received from ATEM device containing upstream keyer DVE settings update
/// </summary>
[Command("KeDV")]
public partial class MixEffectKeyDigitalVideoEffectsUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    [DeserializedField(0)] private byte _mixEffectId;

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    [DeserializedField(1)] private byte _keyerId;

    /// <summary>
    /// Horizontal size scale factor
    /// </summary>
    [DeserializedField(4)] [SerializedType(typeof(uint))] [ScalingFactor(1000.0)]
    private double _sizeX;

    /// <summary>
    /// Vertical size scale factor
    /// </summary>
    [DeserializedField(8)] [SerializedType(typeof(uint))] [ScalingFactor(1000.0)]
    private double _sizeY;

    /// <summary>
    /// Horizontal position offset
    /// </summary>
    [DeserializedField(12)] [SerializedType(typeof(int))] [ScalingFactor(1000.0)]
    private double _positionX;

    /// <summary>
    /// Vertical position offset
    /// </summary>
    [DeserializedField(16)] [SerializedType(typeof(int))] [ScalingFactor(1000.0)]
    private double _positionY;

    /// <summary>
    /// Rotation angle in degrees
    /// </summary>
    [DeserializedField(20)] [SerializedType(typeof(int))] [ScalingFactor(10.0)]
    private double _rotation;

    /// <summary>
    /// Whether border effect is enabled
    /// </summary>
    [DeserializedField(24)] private bool _borderEnabled;

    /// <summary>
    /// Whether shadow effect is enabled
    /// </summary>
    [DeserializedField(25)] private bool _shadowEnabled;

    /// <summary>
    /// Type of border bevel effect
    /// </summary>
    [DeserializedField(26)] private BorderBevel _borderBevel;

    /// <summary>
    /// Outer border width
    /// </summary>
    [DeserializedField(28)] [SerializedType(typeof(ushort))] [ScalingFactor(100.0)]
    private double _borderOuterWidth;

    /// <summary>
    /// Inner border width
    /// </summary>
    [DeserializedField(30)] [SerializedType(typeof(ushort))] [ScalingFactor(100.0)]
    private double _borderInnerWidth;

    /// <summary>
    /// Outer border softness
    /// </summary>
    [DeserializedField(32)] private byte _borderOuterSoftness;

    /// <summary>
    /// Inner border softness
    /// </summary>
    [DeserializedField(33)] private byte _borderInnerSoftness;

    /// <summary>
    /// Border bevel softness
    /// </summary>
    [DeserializedField(34)] private byte _borderBevelSoftness;

    /// <summary>
    /// Border bevel position
    /// </summary>
    [DeserializedField(35)] private byte _borderBevelPosition;

    /// <summary>
    /// Border opacity
    /// </summary>
    [DeserializedField(36)] private byte _borderOpacity;

    /// <summary>
    /// Border color hue
    /// </summary>
    [DeserializedField(38)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderHue;

    /// <summary>
    /// Border color saturation
    /// </summary>
    [DeserializedField(40)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderSaturation;

    /// <summary>
    /// Border color luminance
    /// </summary>
    [DeserializedField(42)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _borderLuma;

    /// <summary>
    /// Light source direction angle
    /// </summary>
    [DeserializedField(44)] [SerializedType(typeof(ushort))] [ScalingFactor(10.0)]
    private double _lightSourceDirection;

    /// <summary>
    /// Light source altitude
    /// </summary>
    [DeserializedField(46)] [SerializedType(typeof(byte))]
    private double _lightSourceAltitude;

    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    [DeserializedField(47)] private bool _maskEnabled;

    /// <summary>
    /// Top edge of mask
    /// </summary>
    [DeserializedField(48)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskTop;

    /// <summary>
    /// Bottom edge of mask
    /// </summary>
    [DeserializedField(50)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskBottom;

    /// <summary>
    /// Left edge of mask
    /// </summary>
    [DeserializedField(52)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskLeft;

    /// <summary>
    /// Right edge of mask
    /// </summary>
    [DeserializedField(54)] [SerializedType(typeof(ushort))] [ScalingFactor(1000.0)]
    private double _maskRight;

    /// <summary>
    /// Transition rate in frames
    /// </summary>
    [DeserializedField(56)] private byte _rate;


    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get or create the mix effect
        var mixEffect = state.Video.MixEffects[MixEffectId];

        // Get or create the upstream keyer
        var keyer = mixEffect.UpstreamKeyers.GetOrCreate(KeyerId);
        keyer.Id = KeyerId;

        // Update the DVE settings
        keyer.DigitalVideoEffectsSettings.SizeX = SizeX;
        keyer.DigitalVideoEffectsSettings.SizeY = SizeY;
        keyer.DigitalVideoEffectsSettings.Location = new PointF((float)PositionX, (float)PositionY);
        keyer.DigitalVideoEffectsSettings.Rotation = Rotation;
        keyer.DigitalVideoEffectsSettings.ShadowEnabled = ShadowEnabled;
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
        keyer.DigitalVideoEffectsSettings.Border.LightSourceDirection = LightSourceDirection;
        keyer.DigitalVideoEffectsSettings.Border.LightSourceAltitude = LightSourceAltitude;
        keyer.DigitalVideoEffectsSettings.MaskEnabled = MaskEnabled;
        keyer.DigitalVideoEffectsSettings.MaskTop = MaskTop;
        keyer.DigitalVideoEffectsSettings.MaskBottom = MaskBottom;
        keyer.DigitalVideoEffectsSettings.MaskLeft = MaskLeft;
        keyer.DigitalVideoEffectsSettings.MaskRight = MaskRight;
        keyer.DigitalVideoEffectsSettings.Rate = Rate;
    }
}
