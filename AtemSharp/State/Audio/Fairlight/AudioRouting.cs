using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class AudioRouting
{
    public Dictionary<ulong, AudioRoutingEntry> Outputs { get; } = new();
    public Dictionary<ulong, AudioRoutingEntry> Sources { get; } = new();
}
