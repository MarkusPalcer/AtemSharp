using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;
using AtemSharp.Types.Border;

namespace AtemSharp.State.Video.SuperSource;

[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class SuperSource
{
    public SuperSource()
    {
        Boxes = new ItemCollection<byte, SuperSourceBox>(id => new SuperSourceBox
        {
            Id = id,
            SuperSourceId = Id
        });
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public byte Id { get; internal init; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, SuperSourceBox> Boxes { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public Border Border { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ShadowProperties Shadow { get; } = new();

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ushort FillSource { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ushort CutSource { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ArtOption Option { get; internal set; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public PreMultipliedKey PreMultipliedKey { get; } = new();

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{Id}";
}
