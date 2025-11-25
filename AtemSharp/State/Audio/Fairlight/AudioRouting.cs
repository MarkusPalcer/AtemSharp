namespace AtemSharp.State.Audio.Fairlight;

public class AudioRouting
{
    public Dictionary<ulong, AudioRoutingEntry> Outputs { get; } = new();
    public Dictionary<ulong, AudioRoutingEntry> Sources { get; } = new();
}
