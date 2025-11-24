namespace AtemSharp.State;

public class SuperSource
{
    public byte Id { get; set; }
    public Dictionary<byte, SuperSourceBox> Boxes { get; } = new();
    public SuperSourceBorderProperties Border { get; internal set; } = new();
    public ushort FillSource { get; set; }
    public ushort CutSource { get; set; }
    public ArtOption Option { get; set; }
    public bool PreMultiplied { get; set; }
}
