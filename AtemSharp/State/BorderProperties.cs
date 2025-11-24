namespace AtemSharp.State;

public class BorderProperties
{
    public double OuterWidth { get; internal set; }
    public double InnerWidth { get; internal set; }
    public byte OuterSoftness { get; internal set; }
    public byte InnerSoftness { get; internal set; }
    public byte BevelSoftness { get; internal set; }
    public byte BevelPosition { get; internal set; }
    public byte Opacity { get; internal set; }
    public double Hue { get; internal set; }
    public double Saturation { get; internal set; }
    public double Luma { get; internal set; }
}