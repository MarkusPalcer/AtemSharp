using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Media;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MediaState
{
    public MediaPoolEntry[] Frames { get; internal set; } = [];

    public MediaPoolEntry[] Clips { get; internal set; } = [];

    public MediaPlayer[] Players { get; internal set; } = [];
}
