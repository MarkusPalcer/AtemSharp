using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

public class LimiterParameters
{
    internal byte Flag { get; set; } = 0;

    private double _release;
    private double _hold;
    private double _attack;
    private double _threshold;
    private bool _limiterEnabled;

    public LimiterParameters(Limiter limiter)
    {
        _release = limiter.Release;
        _hold = limiter.Hold;
        _attack = limiter.Attack;
        _threshold = limiter.Threshold;
        _limiterEnabled = limiter.Enabled;
    }

    internal LimiterParameters()
    {

    }

    public double Release
    {
        get { return _release; }
        set
        {
            _release = value;
            Flag |= 1 << 4;
        }
    }

    public double Hold
    {
        get { return _hold; }
        set
        {
            _hold = value;
            Flag |= 1 << 3;
        }
    }

    public double Attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
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

    public bool LimiterEnabled
    {
        get { return _limiterEnabled; }
        set
        {
            _limiterEnabled = value;
            Flag |= 1 << 0;
        }
    }

    internal void ApplyTo(Limiter limiter)
    {
        limiter.Enabled = LimiterEnabled;
        limiter.Threshold = Threshold;
        limiter.Attack = Attack;
        limiter.Hold = Hold;
        limiter.Release = Release;
    }
}
