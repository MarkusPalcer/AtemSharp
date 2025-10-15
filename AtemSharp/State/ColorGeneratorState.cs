namespace AtemSharp.State;

// TODO: Extract color base class and use that in other places that represent HSL colors (e.g. USK border color)
public class ColorGeneratorState
{
    public double Hue { get; set; }
    public double Saturation { get; set; }
    public double Luma { get; set; }
}
