namespace AtemSharp.State.Audio.Fairlight;

public class AudioRouting
{
    public Dictionary<ulong, AudioRoutingEntry> Outputs = new();
    public Dictionary<ulong, AudioRoutingEntry> Sources = new();
}