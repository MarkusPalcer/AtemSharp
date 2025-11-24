namespace AtemSharp.State.Audio.Fairlight;

public class Dynamics
{
    public double MakeUpGain { get; internal set; }

    public Expander Expander { get;  } = new();
    public Compressor Compressor { get;  } = new();

    public Limiter Limiter { get; } = new();
}