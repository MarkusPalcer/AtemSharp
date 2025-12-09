using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;
using AtemSharp.Types.Border;

namespace AtemSharp.State.Video.SuperSource;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SuperSource : ItemWithId<int>
{
    internal override void SetId(int id) => Id = (byte)id;
    public byte Id { get; internal set; }

    public Dictionary<byte, SuperSourceBox> Boxes { get; } = [];
    public Border Border { get; } = new();

    public ShadowProperties Shadow { get; } = new();

    public ushort FillSource { get; internal set; }
    public ushort CutSource { get; internal set; }
    public ArtOption Option { get; internal set; }
    public bool PreMultiplied { get; internal set; }
    public double Clip { get; internal set; }
    public double Gain { get; internal set; }
    public bool InvertKey { get; internal set; }
}
