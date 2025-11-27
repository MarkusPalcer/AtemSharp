using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class AuxiliaryOutput : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;

    public byte Id { get; internal set; }

    public ushort Source { get; internal set; }
}
