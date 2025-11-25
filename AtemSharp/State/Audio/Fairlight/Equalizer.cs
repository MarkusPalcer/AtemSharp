namespace AtemSharp.State.Audio.Fairlight;

public abstract class Equalizer
{
    public bool Enabled { get; internal set; }
    public double Gain { get; internal set; }
}
