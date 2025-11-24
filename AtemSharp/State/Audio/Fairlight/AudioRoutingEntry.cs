namespace AtemSharp.State.Audio.Fairlight;

public class AudioRoutingEntry
{
    public uint Id { get; internal set; }
    public AudioChannelPair ChannelPair { get; internal set; }
    public ushort InternalPortType { get; internal set; }
    public ushort ExternalPortType { get; internal set; }
    public string Name { get; internal set; }
}