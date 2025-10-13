using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update DVE settings for an upstream keyer
/// </summary>
[Command("CKDV")]
public class MixEffectKeyDVECommand : SerializedCommand
{
    private uint _dveFlag; // Use uint for 32-bit flags instead of base ushort Flag
    
    private double _sizeX;
    private double _sizeY;
    private double _positionX;
    private double _positionY;
    private double _rotation;
    private bool _borderEnabled;
    private bool _shadowEnabled;
    private BorderBevel _borderBevel;
    private double _borderOuterWidth;
    private double _borderInnerWidth;
    private double _borderOuterSoftness;
    private double _borderInnerSoftness;
    private double _borderBevelSoftness;
    private double _borderBevelPosition;
    private double _borderOpacity;
    private double _borderHue;
    private double _borderSaturation;
    private double _borderLuma;
    private double _lightSourceDirection;
    private double _lightSourceAltitude;
    private bool _maskEnabled;
    private double _maskTop;
    private double _maskBottom;
    private double _maskLeft;
    private double _maskRight;
    private int _rate;

    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; }

    /// <summary>
    /// Upstream keyer index (0-based)
    /// </summary>
    public int KeyerId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="mixEffectId">Mix effect index (0-based)</param>
    /// <param name="keyerId">Upstream keyer index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if mix effect or keyer not available</exception>
    public MixEffectKeyDVECommand(int mixEffectId, int keyerId, AtemState currentState)
    {
        MixEffectId = mixEffectId;
        KeyerId = keyerId;

        // If no video state or mix effect doesn't exist, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            !mixEffect.UpstreamKeyers.TryGetValue(keyerId, out var keyer) ||
            keyer.DVESettings == null)
        {
            // Set default values and flags (like TypeScript pattern)
            SizeX = 100.0;
            SizeY = 100.0;
            PositionX = 0.0;
            PositionY = 0.0;
            Rotation = 0.0;
            BorderEnabled = false;
            ShadowEnabled = false;
            BorderBevel = BorderBevel.None;
            BorderOuterWidth = 0.0;
            BorderInnerWidth = 0.0;
            BorderOuterSoftness = 0.0;
            BorderInnerSoftness = 0.0;
            BorderBevelSoftness = 0.0;
            BorderBevelPosition = 0.0;
            BorderOpacity = 0.0;
            BorderHue = 0.0;
            BorderSaturation = 0.0;
            BorderLuma = 0.0;
            LightSourceDirection = 0.0;
            LightSourceAltitude = 0.0;
            MaskEnabled = false;
            MaskTop = 0.0;
            MaskBottom = 0.0;
            MaskLeft = 0.0;
            MaskRight = 0.0;
            Rate = 0;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        var dve = keyer.DVESettings;
        _sizeX = dve.SizeX;
        _sizeY = dve.SizeY;
        _positionX = dve.PositionX;
        _positionY = dve.PositionY;
        _rotation = dve.Rotation;
        _borderEnabled = dve.BorderEnabled;
        _shadowEnabled = dve.ShadowEnabled;
        _borderBevel = dve.BorderBevel;
        _borderOuterWidth = dve.BorderOuterWidth;
        _borderInnerWidth = dve.BorderInnerWidth;
        _borderOuterSoftness = dve.BorderOuterSoftness;
        _borderInnerSoftness = dve.BorderInnerSoftness;
        _borderBevelSoftness = dve.BorderBevelSoftness;
        _borderBevelPosition = dve.BorderBevelPosition;
        _borderOpacity = dve.BorderOpacity;
        _borderHue = dve.BorderHue;
        _borderSaturation = dve.BorderSaturation;
        _borderLuma = dve.BorderLuma;
        _lightSourceDirection = dve.LightSourceDirection;
        _lightSourceAltitude = dve.LightSourceAltitude;
        _maskEnabled = dve.MaskEnabled;
        _maskTop = dve.MaskTop;
        _maskBottom = dve.MaskBottom;
        _maskLeft = dve.MaskLeft;
        _maskRight = dve.MaskRight;
        _rate = dve.Rate;
    }

    /// <summary>
    /// Horizontal size scale factor (0-1000)
    /// </summary>
    public double SizeX
    {
        get => _sizeX;
        set
        {
            _sizeX = value;
            _dveFlag |= 1U << 0;
        }
    }

    /// <summary>
    /// Vertical size scale factor (0-1000)
    /// </summary>
    public double SizeY
    {
        get => _sizeY;
        set
        {
            _sizeY = value;
            _dveFlag |= 1U << 1;
        }
    }

    /// <summary>
    /// Horizontal position offset (-1000 to 1000)
    /// </summary>
    public double PositionX
    {
        get => _positionX;
        set
        {
            _positionX = value;
            _dveFlag |= 1U << 2;
        }
    }

    /// <summary>
    /// Vertical position offset (-1000 to 1000)
    /// </summary>
    public double PositionY
    {
        get => _positionY;
        set
        {
            _positionY = value;
            _dveFlag |= 1U << 3;
        }
    }

    /// <summary>
    /// Rotation angle in degrees (-36000 to 36000)
    /// </summary>
    public double Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            _dveFlag |= 1U << 4;
        }
    }

    /// <summary>
    /// Whether border effect is enabled
    /// </summary>
    public bool BorderEnabled
    {
        get => _borderEnabled;
        set
        {
            _borderEnabled = value;
            _dveFlag |= 1U << 5;
        }
    }

    /// <summary>
    /// Whether shadow effect is enabled
    /// </summary>
    public bool ShadowEnabled
    {
        get => _shadowEnabled;
        set
        {
            _shadowEnabled = value;
            _dveFlag |= 1U << 6;
        }
    }

    /// <summary>
    /// Type of border bevel effect
    /// </summary>
    public BorderBevel BorderBevel
    {
        get => _borderBevel;
        set
        {
            _borderBevel = value;
            _dveFlag |= 1U << 7;
        }
    }

    /// <summary>
    /// Outer border width (0-16)
    /// </summary>
    public double BorderOuterWidth
    {
        get => _borderOuterWidth;
        set
        {
            _borderOuterWidth = value;
            _dveFlag |= 1U << 8;
        }
    }

    /// <summary>
    /// Inner border width (0-16)
    /// </summary>
    public double BorderInnerWidth
    {
        get => _borderInnerWidth;
        set
        {
            _borderInnerWidth = value;
            _dveFlag |= 1U << 9;
        }
    }

    /// <summary>
    /// Outer border softness (0-100)
    /// </summary>
    public double BorderOuterSoftness
    {
        get => _borderOuterSoftness;
        set
        {
            _borderOuterSoftness = value;
            _dveFlag |= 1U << 10;
        }
    }

    /// <summary>
    /// Inner border softness (0-100)
    /// </summary>
    public double BorderInnerSoftness
    {
        get => _borderInnerSoftness;
        set
        {
            _borderInnerSoftness = value;
            _dveFlag |= 1U << 11;
        }
    }

    /// <summary>
    /// Border bevel softness (0-100)
    /// </summary>
    public double BorderBevelSoftness
    {
        get => _borderBevelSoftness;
        set
        {
            _borderBevelSoftness = value;
            _dveFlag |= 1U << 12;
        }
    }

    /// <summary>
    /// Border bevel position (0-100)
    /// </summary>
    public double BorderBevelPosition
    {
        get => _borderBevelPosition;
        set
        {
            _borderBevelPosition = value;
            _dveFlag |= 1U << 13;
        }
    }

    /// <summary>
    /// Border opacity (0-100)
    /// </summary>
    public double BorderOpacity
    {
        get => _borderOpacity;
        set
        {
            _borderOpacity = value;
            _dveFlag |= 1U << 14;
        }
    }

    /// <summary>
    /// Border color hue (0-360 degrees)
    /// </summary>
    public double BorderHue
    {
        get => _borderHue;
        set
        {
            _borderHue = value;
            _dveFlag |= 1U << 15;
        }
    }

    /// <summary>
    /// Border color saturation (0-100)
    /// </summary>
    public double BorderSaturation
    {
        get => _borderSaturation;
        set
        {
            _borderSaturation = value;
            _dveFlag |= 1U << 16;
        }
    }

    /// <summary>
    /// Border color luminance (0-100)
    /// </summary>
    public double BorderLuma
    {
        get => _borderLuma;
        set
        {
            _borderLuma = value;
            _dveFlag |= 1U << 17;
        }
    }

    /// <summary>
    /// Light source direction angle (0-360 degrees)
    /// </summary>
    public double LightSourceDirection
    {
        get => _lightSourceDirection;
        set
        {
            _lightSourceDirection = value;
            _dveFlag |= 1U << 18;
        }
    }

    /// <summary>
    /// Light source altitude (0-100)
    /// </summary>
    public double LightSourceAltitude
    {
        get => _lightSourceAltitude;
        set
        {
            _lightSourceAltitude = value;
            _dveFlag |= 1U << 19;
        }
    }

    /// <summary>
    /// Whether masking is enabled
    /// </summary>
    public bool MaskEnabled
    {
        get => _maskEnabled;
        set
        {
            _maskEnabled = value;
            _dveFlag |= 1U << 20;
        }
    }

    /// <summary>
    /// Top edge of mask (0-100)
    /// </summary>
    public double MaskTop
    {
        get => _maskTop;
        set
        {
            _maskTop = value;
            _dveFlag |= 1U << 21;
        }
    }

    /// <summary>
    /// Bottom edge of mask (0-100)
    /// </summary>
    public double MaskBottom
    {
        get => _maskBottom;
        set
        {
            _maskBottom = value;
            _dveFlag |= 1U << 22;
        }
    }

    /// <summary>
    /// Left edge of mask (0-100)
    /// </summary>
    public double MaskLeft
    {
        get => _maskLeft;
        set
        {
            _maskLeft = value;
            _dveFlag |= 1U << 23;
        }
    }

    /// <summary>
    /// Right edge of mask (0-100)
    /// </summary>
    public double MaskRight
    {
        get => _maskRight;
        set
        {
            _maskRight = value;
            _dveFlag |= 1U << 24;
        }
    }

    /// <summary>
    /// Transition rate in frames
    /// </summary>
    public int Rate
    {
        get => _rate;
        set
        {
            _rate = value;
            _dveFlag |= 1U << 25;
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(64);
        using var writer = new BinaryWriter(memoryStream);

        writer.WriteUInt32BigEndian(_dveFlag);
        writer.Write((byte)MixEffectId);
        writer.Write((byte)KeyerId);
        writer.Write((ushort)0); // Padding

        // Size values - scale by 1000
        writer.WriteUInt32BigEndian((uint)(SizeX * 1000));
        writer.WriteUInt32BigEndian((uint)(SizeY * 1000));

        // Position values - scale by 1000 and treat as signed
        writer.WriteInt32BigEndian((int)(PositionX * 1000));
        writer.WriteInt32BigEndian((int)(PositionY * 1000));

        // Rotation - scale by 1000 and treat as signed  
        writer.WriteInt32BigEndian((int)(Rotation * 10));

        writer.WriteBoolean(BorderEnabled);
        writer.WriteBoolean(ShadowEnabled);
        writer.Write((byte)BorderBevel);
        writer.Write((byte)0); // Padding

        // Border widths - scale by 65536 and convert to uint16
        writer.WriteUInt16BigEndian((ushort)(BorderOuterWidth * 65536 / 16));
        writer.WriteUInt16BigEndian((ushort)(BorderInnerWidth * 65536 / 16));

        // Softness and position values as signed bytes
        writer.Write((sbyte)BorderOuterSoftness);
        writer.Write((sbyte)BorderInnerSoftness);
        writer.Write((sbyte)BorderBevelSoftness);
        writer.Write((sbyte)BorderBevelPosition);

        writer.Write((sbyte)BorderOpacity);
        writer.Write((byte)0); // Padding

        // Color values - based on reverse engineering: deserialization uses * 0.1, so use / 0.1
        writer.WriteUInt16BigEndian((ushort)(BorderHue / 0.1));
        writer.WriteUInt16BigEndian((ushort)(BorderSaturation / 0.1));
        writer.WriteUInt16BigEndian((ushort)(BorderLuma / 0.1));

        // Light source - direction scaled inverse of deserialization
        writer.WriteUInt16BigEndian((ushort)(LightSourceDirection / 0.1));
        writer.Write((byte)LightSourceAltitude);

        writer.WriteBoolean(MaskEnabled);

        // Mask values - inverse of deserialization: rawValue * 100.0 / 65536.0 / 1.515
        writer.WriteUInt16BigEndian((ushort)(MaskTop * 65536.0 * 1.515 / 100.0));
        writer.WriteUInt16BigEndian((ushort)(MaskBottom * 65536.0 * 1.515 / 100.0));
        writer.WriteUInt16BigEndian((ushort)(MaskLeft * 65536.0 * 1.515 / 100.0));
        writer.WriteUInt16BigEndian((ushort)(MaskRight * 65536.0 * 1.515 / 100.0));

        writer.Write((byte)Rate);
        writer.Write((byte)0); // Padding
        writer.Write((ushort)0); // Padding

        return memoryStream.ToArray();
    }
}