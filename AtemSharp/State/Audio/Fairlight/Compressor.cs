namespace AtemSharp.State.Audio.Fairlight;

public class Compressor
{
    public bool Enabled { get; internal set; }
    public double Threshold { get; internal set; }
    public double Ratio { get; internal set; }
    public double Attack { get; internal set; }
    public double Hold { get; internal set; }
    public double Release { get; internal set; }
}
