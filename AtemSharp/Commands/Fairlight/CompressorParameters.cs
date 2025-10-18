using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

public class CompressorParameters
{
    private double _release;

    private double _hold;

    private double _attack;

    private double _ratio;

    private double _threshold;

    private bool _compressorEnabled;

    public CompressorParameters() {}

    public CompressorParameters(Compressor compressor)
    {
        CompressorEnabled = compressor.Enabled;
        Threshold = compressor.Threshold;
        Ratio = compressor.Ratio;
        Attack = compressor.Attack;
        Hold = compressor.Hold;
        Release = compressor.Release;
    }

    public byte Flag { get; internal set; } = 0;

    public double Release
    {
        get { return _release; }
        set
        {
            _release = value;
            Flag |= 1 << 5;
        }
    }

    public double Hold
    {
        get { return _hold; }
        set
        {
            _hold = value;
            Flag |= 1 << 4;
        }
    }

    public double Attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
            Flag |= 1 << 3;
        }
    }

    public double Ratio
    {
        get { return _ratio; }
        set
        {
            _ratio = value;
            Flag |= 1 << 2;
        }
    }

    public double Threshold
    {
        get { return _threshold; }
        set
        {
            _threshold = value;
            Flag |= 1 << 1;
        }
    }

    public bool CompressorEnabled
    {
        get { return _compressorEnabled; }
        set
        {
            _compressorEnabled = value;
            Flag |= 1 << 0;
        }
    }

    public void ApplyTo(Compressor compressor)
    {
        compressor.Enabled = CompressorEnabled;
        compressor.Threshold = Threshold;
        compressor.Ratio = Ratio;
        compressor.Attack = Attack;
        compressor.Hold = Hold;
        compressor.Release = Release;
    }
}
