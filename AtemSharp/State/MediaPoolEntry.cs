namespace AtemSharp.State;

public class MediaPoolEntry
{
    public byte Id { get; set; }

    public bool IsUsed { get; set; }
    public string Hash { get; set; }
    public string FileName { get; set; }
}
