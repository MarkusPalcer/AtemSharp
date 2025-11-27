using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SoloProperties
{
    public bool Solo { get; internal set; }
    public ushort Index { get; internal set; }
    public long Source { get; internal set; }
}
