using AtemSharp.State.Info;
using AtemSharp.State.Video.MixEffect;
using AtemSharp.State.Video.MixEffect.Transition;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set DVE transition settings for a mix effect
/// </summary>
[Command("CTDv")]
public class TransitionDigitalVideoEffectCommand(MixEffect mixEffect) : SerializedCommand
{
    private readonly int _mixEffectId = mixEffect.Id;
    private int _rate = mixEffect.TransitionSettings.DigitalVideoEffect.Rate;
    private int _logoRate = mixEffect.TransitionSettings.DigitalVideoEffect.LogoRate;
    private DigitalVideoEffect _style = mixEffect.TransitionSettings.DigitalVideoEffect.Style;
    private int _fillSource = mixEffect.TransitionSettings.DigitalVideoEffect.FillSource;
    private int _keySource = mixEffect.TransitionSettings.DigitalVideoEffect.KeySource;
    private bool _enableKey = mixEffect.TransitionSettings.DigitalVideoEffect.EnableKey;
    private bool _preMultiplied = mixEffect.TransitionSettings.DigitalVideoEffect.PreMultiplied;
    private double _clip = mixEffect.TransitionSettings.DigitalVideoEffect.Clip;
    private double _gain = mixEffect.TransitionSettings.DigitalVideoEffect.Gain;
    private bool _invertKey = mixEffect.TransitionSettings.DigitalVideoEffect.InvertKey;
    private bool _reverse = mixEffect.TransitionSettings.DigitalVideoEffect.Reverse;
    private bool _flipFlop = mixEffect.TransitionSettings.DigitalVideoEffect.FlipFlop;


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

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(20);
        using var writer = new BinaryWriter(memoryStream);

        writer.WriteUInt16BigEndian((ushort)Flag); // Flag as 16-bit big endian (matches TypeScript)
        writer.Write((byte)_mixEffectId); // Mix effect index
        writer.Write((byte)Rate); // Rate
        writer.Write((byte)LogoRate); // Logo rate
        writer.Write((byte)Style); // Style

        writer.WriteUInt16BigEndian((ushort)FillSource); // Fill source
        writer.WriteUInt16BigEndian((ushort)KeySource); // Key source

        writer.WriteBoolean(EnableKey); // Enable key
        writer.WriteBoolean(PreMultiplied); // Pre-multiplied
        writer.WriteUInt16BigEndian((ushort)Math.Round(Clip * 10.0)); // Clip (multiply by 10)
        writer.WriteUInt16BigEndian((ushort)Math.Round(Gain * 10.0)); // Gain (multiply by 10)
        writer.WriteBoolean(InvertKey); // Invert key
        writer.WriteBoolean(Reverse); // Reverse
        writer.WriteBoolean(FlipFlop); // Flip flop
        writer.Pad(1); // 1 byte padding to reach 20 bytes

        return memoryStream.ToArray();
    }
}
