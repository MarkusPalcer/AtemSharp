using AtemSharp.Enums;

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

public class SuperSourceBorderProperties : BorderProperties
{
    public bool Enabled { get; internal set; }
    public BorderBevel Bevel { get; internal set; }
    public double LightSourceDirection { get;internal set; }
    public double LightSourceAltitude { get; internal set; }
}
