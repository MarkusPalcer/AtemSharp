namespace AtemSharp.Types;

public class PreMultipliedKey
{
    public bool Enabled { get; set; }
    public double Clip { get; internal set; }
    public double Gain { get; internal set; }
    public bool Inverted { get; internal set; }
}
