namespace AtemSharp.State.Video;

public class AuxiliaryOutput : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;

    public byte Id { get; internal set; }

    public ushort Source { get; internal set; }
}
