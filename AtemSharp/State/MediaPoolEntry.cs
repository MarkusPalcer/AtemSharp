namespace AtemSharp.State;

public class MediaPoolEntry
{
    public byte Id { get; set; }
    public bool IsUsed { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Hash { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public ushort FrameCount { get; set; }
}
