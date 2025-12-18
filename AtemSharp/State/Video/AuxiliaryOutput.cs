using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Video;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class AuxiliaryOutput
{
    public byte Id { get; internal init; }

    public ushort Source { get; internal set; }
}
