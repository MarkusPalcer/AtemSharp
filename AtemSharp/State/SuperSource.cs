namespace AtemSharp.State;

public class SuperSource
{
    public byte Id { get; set; }
    public Dictionary<byte, SuperSourceBox> Boxes { get; } = new();
}