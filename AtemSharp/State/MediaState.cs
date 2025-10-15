namespace AtemSharp.State;

public class MediaState
{
    public MediaPoolEntry[] Frames { get; internal set; } = [];

    public MediaPoolEntry[] Clips { get; internal set; } = [];
}
