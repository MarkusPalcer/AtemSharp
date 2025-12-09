namespace AtemSharp.Types;

public class Shadow : ShadowProperties
{
    public bool Enabled { get; set; }
}

public class ShadowProperties
{
    public double LightSourceDirection { get;internal set; }
    public double LightSourceAltitude { get; internal set; }
}
