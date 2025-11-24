namespace AtemSharp.State;

// TODO: Extract color base class and use that in other places that represent HSL colors (e.g. USK border color)
public class ColorGeneratorState
{
    public double Hue { get; internal set; }
    public double Saturation { get; internal set; }
    public double Luma { get; internal set; }
}
