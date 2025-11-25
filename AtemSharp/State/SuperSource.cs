namespace AtemSharp.State;

public class SuperSource : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;
    public byte Id { get; internal set; }
    public Dictionary<byte, SuperSourceBox> Boxes { get; } = new();
    public SuperSourceBorderProperties Border { get; internal set; } = new();
    public ushort FillSource { get; internal set; }
    public ushort CutSource { get; internal set; }
    public ArtOption Option { get; internal set; }
    public bool PreMultiplied { get; internal set; }
    public double Clip { get; internal set; }
    public double Gain { get; internal set; }
    public bool InvertKey { get; internal set; }
}
