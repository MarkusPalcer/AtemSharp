namespace AtemSharp.State.Media;

public class MediaState
{
    public MediaPoolEntry[] Frames { get; internal set; } = [];

    public MediaPoolEntry[] Clips { get; internal set; } = [];

    public MediaPlayer[] Players { get; internal set; } = [];
}
