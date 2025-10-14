using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Key;

/// <summary>
/// Command to update advanced chroma key properties for an upstream keyer
/// </summary>
[Command("CACK")]
public class MixEffectKeyAdvancedChromaPropertiesCommand : SerializedCommand
{
    private double _foregroundLevel;
    private double _backgroundLevel;
    private double _keyEdge;
    private double _spillSuppression;
    private double _flareSuppression;
    private double _brightness;
    private double _contrast;
    private double _saturation;
    private double _red;
    private double _green;
    private double _blue;

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
    public MixEffectKeyAdvancedChromaPropertiesCommand(int mixEffectId, int keyerId, AtemState currentState)
    {
        MixEffectId = mixEffectId;
        KeyerId = keyerId;

        // If no video state or mix effect doesn't exist, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            !mixEffect.UpstreamKeyers.TryGetValue(keyerId, out var keyer) ||
            keyer.AdvancedChromaSettings?.Properties == null)
        {
            // Set default values and flags (like TypeScript pattern)
            ForegroundLevel = 0.0;
            BackgroundLevel = 0.0;
            KeyEdge = 0.0;
            SpillSuppression = 0.0;
            FlareSuppression = 0.0;
            Brightness = 0.0;
            Contrast = 0.0;
            Saturation = 0.0;
            Red = 0.0;
            Green = 0.0;
            Blue = 0.0;
            return;
        }

        var properties = keyer.AdvancedChromaSettings.Properties;
        
        // Initialize from current state (direct field access = no flags set)
        _foregroundLevel = properties.ForegroundLevel;
        _backgroundLevel = properties.BackgroundLevel;
        _keyEdge = properties.KeyEdge;
        _spillSuppression = properties.SpillSuppression;
        _flareSuppression = properties.FlareSuppression;
        _brightness = properties.Brightness;
        _contrast = properties.Contrast;
        _saturation = properties.Saturation;
        _red = properties.Red;
        _green = properties.Green;
        _blue = properties.Blue;
    }

    /// <summary>
    /// Foreground level value
    /// </summary>
    public double ForegroundLevel
    {
        get => _foregroundLevel;
        set
        {
            _foregroundLevel = value;
            Flag |= 1 << 0;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Background level value
    /// </summary>
    public double BackgroundLevel
    {
        get => _backgroundLevel;
        set
        {
            _backgroundLevel = value;
            Flag |= 1 << 1;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Key edge value
    /// </summary>
    public double KeyEdge
    {
        get => _keyEdge;
        set
        {
            _keyEdge = value;
            Flag |= 1 << 2;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Spill suppression value
    /// </summary>
    public double SpillSuppression
    {
        get => _spillSuppression;
        set
        {
            _spillSuppression = value;
            Flag |= 1 << 3;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Flare suppression value
    /// </summary>
    public double FlareSuppression
    {
        get => _flareSuppression;
        set
        {
            _flareSuppression = value;
            Flag |= 1 << 4;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Brightness adjustment value
    /// </summary>
    public double Brightness
    {
        get => _brightness;
        set
        {
            _brightness = value;
            Flag |= 1 << 5;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Contrast adjustment value
    /// </summary>
    public double Contrast
    {
        get => _contrast;
        set
        {
            _contrast = value;
            Flag |= 1 << 6;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Saturation adjustment value
    /// </summary>
    public double Saturation
    {
        get => _saturation;
        set
        {
            _saturation = value;
            Flag |= 1 << 7;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Red color adjustment value
    /// </summary>
    public double Red
    {
        get => _red;
        set
        {
            _red = value;
            Flag |= 1 << 8;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Green color adjustment value
    /// </summary>
    public double Green
    {
        get => _green;
        set
        {
            _green = value;
            Flag |= 1 << 9;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Blue color adjustment value
    /// </summary>
    public double Blue
    {
        get => _blue;
        set
        {
            _blue = value;
            Flag |= 1 << 10;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(28);
        using var writer = new BinaryWriter(memoryStream);

        // Write flag as 16-bit value (matches TypeScript buffer.writeUInt16BE pattern)
        writer.WriteUInt16BigEndian((ushort)Flag);
        writer.Write((byte)MixEffectId);
        writer.Write((byte)KeyerId);

        // Write all property values as 16-bit integers (scaled by 10 to match TypeScript implementation)
        writer.WriteUInt16BigEndian((ushort)Math.Round(ForegroundLevel * 10));
        writer.WriteUInt16BigEndian((ushort)Math.Round(BackgroundLevel * 10));
        writer.WriteUInt16BigEndian((ushort)Math.Round(KeyEdge * 10));
        
        writer.WriteUInt16BigEndian((ushort)Math.Round(SpillSuppression * 10));
        writer.WriteUInt16BigEndian((ushort)Math.Round(FlareSuppression * 10));
        
        // Color adjustment values can be negative, so use signed 16-bit (also scaled by 10)
        writer.WriteInt16BigEndian((short)Math.Round(Brightness * 10));
        writer.WriteInt16BigEndian((short)Math.Round(Contrast * 10));
        writer.WriteUInt16BigEndian((ushort)Math.Round(Saturation * 10));
        writer.WriteInt16BigEndian((short)Math.Round(Red * 10));
        writer.WriteInt16BigEndian((short)Math.Round(Green * 10));
        writer.WriteInt16BigEndian((short)Math.Round(Blue * 10));
        
        // Pad to 28 bytes total (we've written 26 bytes, need 2 more)
        writer.Pad(2);
        
        return memoryStream.ToArray();
    }
}