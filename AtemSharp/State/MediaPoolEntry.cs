namespace AtemSharp.State;

public class MediaPoolEntry
{
    public byte Id { get; internal set; }
    public bool IsUsed { get; internal set; }

    public string Name { get; internal set; } = string.Empty;

    public string Hash { get; internal set; } = string.Empty;
    public string FileName { get; internal set; } = string.Empty;
    public ushort FrameCount { get; internal set; }
}
