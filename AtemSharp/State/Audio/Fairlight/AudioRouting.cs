using AtemSharp.Commands.Fairlight.AudioRouting;

namespace AtemSharp.State.Audio.Fairlight;

public class AudioRouting
{
    public Dictionary<ulong, AudioRoutingEntry> Outputs = new();
    public Dictionary<ulong, AudioRoutingEntry> Sources = new();
}

public class AudioRoutingEntry
{
    public uint Id { get; internal set; }
    public AudioChannelPair ChannelPair { get; internal set; }
    public ushort InternalPortType { get; internal set; }
    public ushort ExternalPortType { get; internal set; }
    public string Name { get; internal set; }
}


