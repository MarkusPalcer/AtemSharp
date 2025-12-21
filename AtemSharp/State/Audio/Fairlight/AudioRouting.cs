using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Audio.Fairlight;

public class AudioRouting
{
    internal AudioRouting()
    {
        Outputs = new ItemCollection<ulong, AudioRoutingEntry>(_ => new AudioRoutingEntry());
        Sources = new ItemCollection<ulong, AudioRoutingEntry>(_ => new AudioRoutingEntry());
    }

    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public ItemCollection<ulong, AudioRoutingEntry> Outputs { get; }

    [ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
    public ItemCollection<ulong, AudioRoutingEntry> Sources { get; }
}
