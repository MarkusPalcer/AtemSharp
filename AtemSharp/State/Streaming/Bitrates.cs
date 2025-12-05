using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Streaming;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class Bitrates
{
    public uint Low { get; internal set; }
    public uint High { get; internal set; }
}
