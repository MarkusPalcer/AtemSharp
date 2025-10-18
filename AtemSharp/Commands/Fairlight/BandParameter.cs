using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

public class BandParameter(EqualizerBand band)
{
    private bool _enabled = band.Enabled;
    private byte _shape = band.Shape;
    private byte _frequencyRange = band.FrequencyRange;
    private uint _frequency = band.Frequency;
    private double _gain = band.Gain;
    private double _qFactor = band.QFactor;

    internal byte Flag { get; set; } = 0;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            Flag |= 1 << 0;
        }
    }

    public byte Shape
    {
        get => _shape;
        set
        {
            _shape = value;
            Flag |= 1 << 1;
        }
    }

    public byte FrequencyRange
    {
        get => _frequencyRange;
        set
        {
            _frequencyRange = value;
            Flag |= 1 << 2;
        }
    }

    public uint Frequency
    {
        get => _frequency;
        set
        {
            _frequency = value;
            Flag |= 1 << 3;
        }
    }

    public double Gain
    {
        get => _gain;
        set
        {
            _gain = value;
            Flag |= 1 << 4;
        }
    }

    public double QFactor
    {
        get => _qFactor;
        set
        {
            _qFactor = value;
            Flag |= 1 << 5;
        }
    }
}
