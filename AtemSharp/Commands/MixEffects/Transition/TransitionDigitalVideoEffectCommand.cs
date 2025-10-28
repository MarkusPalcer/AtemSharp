using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set DVE transition settings for a mix effect
/// </summary>
[Command("CTDv")]
public class TransitionDigitalVideoEffectCommand : SerializedCommand
{
    private int _rate;
    private int _logoRate;
    private DigitalVideoEffect _style;
    private int _fillSource;
    private int _keySource;
    private bool _enableKey;
    private bool _preMultiplied;
    private double _clip;
    private double _gain;
    private bool _invertKey;
    private bool _reverse;
    private bool _flipFlop;

    public int MixEffectId { get; }

    public TransitionDigitalVideoEffectCommand(MixEffect mixEffect)
    {
        MixEffectId = mixEffect.Index;

        // Initialize from current state (direct field access = no flags set)
        var dveSettings = mixEffect.TransitionSettings.DigitalVideoEffect;
        _rate = dveSettings.Rate;
        _logoRate = dveSettings.LogoRate;
        _style = dveSettings.Style;
        _fillSource = dveSettings.FillSource;
        _keySource = dveSettings.KeySource;
        _enableKey = dveSettings.EnableKey;
        _preMultiplied = dveSettings.PreMultiplied;
        _clip = dveSettings.Clip;
        _gain = dveSettings.Gain;
        _invertKey = dveSettings.InvertKey;
        _reverse = dveSettings.Reverse;
        _flipFlop = dveSettings.FlipFlop;
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
            Flag |= 1 << 0;
        }
    }

    /// <summary>
    /// Logo/key transition rate in frames
    /// </summary>
    public int LogoRate
    {
        get => _logoRate;
        set
        {
            _logoRate = value;
            Flag |= 1 << 1;
        }
    }

    /// <summary>
    /// DVE effect style
    /// </summary>
    public DigitalVideoEffect Style
    {
        get => _style;
        set
        {
            _style = value;
            Flag |= 1 << 2;
        }
    }

    /// <summary>
    /// Fill source input number
    /// </summary>
    public int FillSource
    {
        get => _fillSource;
        set
        {
            _fillSource = value;
            Flag |= 1 << 3;
        }
    }

    /// <summary>
    /// Key source input number
    /// </summary>
    public int KeySource
    {
        get => _keySource;
        set
        {
            _keySource = value;
            Flag |= 1 << 4;
        }
    }

    /// <summary>
    /// Whether the key is enabled
    /// </summary>
    public bool EnableKey
    {
        get => _enableKey;
        set
        {
            _enableKey = value;
            Flag |= 1 << 5;
        }
    }

    /// <summary>
    /// Whether the key is pre-multiplied
    /// </summary>
    public bool PreMultiplied
    {
        get => _preMultiplied;
        set
        {
            _preMultiplied = value;
            Flag |= 1 << 6;
        }
    }

    /// <summary>
    /// Key clip value (0.0 to 100.0)
    /// </summary>
    public double Clip
    {
        get => _clip;
        set
        {
            _clip = value;
            Flag |= 1 << 7;
        }
    }

    /// <summary>
    /// Key gain value (0.0 to 100.0)
    /// </summary>
    public double Gain
    {
        get => _gain;
        set
        {
            _gain = value;
            Flag |= 1 << 8;
        }
    }

    /// <summary>
    /// Whether the key is inverted
    /// </summary>
    public bool InvertKey
    {
        get => _invertKey;
        set
        {
            _invertKey = value;
            Flag |= 1 << 9;
        }
    }

    /// <summary>
    /// Whether the transition is reversed
    /// </summary>
    public bool Reverse
    {
        get => _reverse;
        set
        {
            _reverse = value;
            Flag |= 1 << 10;
        }
    }

    /// <summary>
    /// Whether flip-flop is enabled
    /// </summary>
    public bool FlipFlop
    {
        get => _flipFlop;
        set
        {
            _flipFlop = value;
            Flag |= 1 << 11;
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(20);
        using var writer = new BinaryWriter(memoryStream);

        writer.WriteUInt16BigEndian((ushort)Flag);  // Flag as 16-bit big endian (matches TypeScript)
        writer.Write((byte)MixEffectId);           // Mix effect index
        writer.Write((byte)Rate);                  // Rate
        writer.Write((byte)LogoRate);              // Logo rate
        writer.Write((byte)Style);                 // Style

        writer.WriteUInt16BigEndian((ushort)FillSource); // Fill source
        writer.WriteUInt16BigEndian((ushort)KeySource);  // Key source

        writer.WriteBoolean(EnableKey);    // Enable key
        writer.WriteBoolean(PreMultiplied); // Pre-multiplied
        writer.WriteUInt16BigEndian((ushort)Math.Round(Clip * 10.0));   // Clip (multiply by 10)
        writer.WriteUInt16BigEndian((ushort)Math.Round(Gain * 10.0));   // Gain (multiply by 10)
        writer.WriteBoolean(InvertKey);     // Invert key
        writer.WriteBoolean(Reverse);       // Reverse
        writer.WriteBoolean(FlipFlop);      // Flip flop
        writer.Pad(1);                              // 1 byte padding to reach 20 bytes

        return memoryStream.ToArray();
    }
}
