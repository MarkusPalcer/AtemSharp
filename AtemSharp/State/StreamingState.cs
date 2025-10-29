namespace AtemSharp.State;

public class StreamingState
{
    public string ServiceName { get; internal set; } = string.Empty;
    public string Url { get; internal set; } = string.Empty;
    public string Key { get; internal set; } = string.Empty;
    public uint Bitrate1 { get; internal set; }
    public uint Bitrate2 { get; internal set; }
}
