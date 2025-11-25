using AtemSharp.State.Border;

namespace AtemSharp.State.Video.SuperSource;

public class SuperSourceBorderProperties : BorderProperties
{
    public bool Enabled { get; internal set; }
    public BorderBevel Bevel { get; internal set; }
    public double LightSourceDirection { get;internal set; }
    public double LightSourceAltitude { get; internal set; }
}
