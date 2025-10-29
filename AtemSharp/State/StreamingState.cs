namespace AtemSharp.State;

public class StreamingState
{
    public string ServiceName { get; internal set; } = string.Empty;
    public string Url { get; internal set; } = string.Empty;
    public string Key { get; internal set; } = string.Empty;

    public Bitrates VideoBitrates { get; } = new();
    public Bitrates AudioBitrates { get; } = new();
}
