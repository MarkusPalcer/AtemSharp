namespace AtemSharp.State;

public class TimeCode
{
    public byte Hours { get; internal set; }
    public byte Minutes { get; internal set; }
    public byte Seconds { get; internal set; }
    public byte Frames { get; internal set; }
}
