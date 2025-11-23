using AtemSharp.Commands.Fairlight.AudioRouting;

namespace AtemSharp.State.Audio.Fairlight;

public class AudioRouting
{
    public Dictionary<ulong, AudioRoutingOutput> Outputs = new();
}

public class AudioRoutingOutput
{
    public uint Id { get; set; }
    public AudioChannelPair ChannelPair { get; set; }
    public ushort InternalPortType { get; set; }
    public ushort ExternalPortType { get; set; }
    public string Name { get; set; }
}
