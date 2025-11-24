namespace AtemSharp.State;

public class MediaPoolSettings
{
    public ushort[] MaxFrames { get; internal set; } = [];
    public ushort UnassignedFrames { get; internal set; }
}
