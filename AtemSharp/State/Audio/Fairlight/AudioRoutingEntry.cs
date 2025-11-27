using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Audio.Fairlight;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class AudioRoutingEntry
{
    public uint Id { get; internal set; }
    public AudioChannelPair ChannelPair { get; internal set; }
    public ushort InternalPortType { get; internal set; }
    public ushort ExternalPortType { get; internal set; }
    public string Name { get; internal set; } = string.Empty;
}
