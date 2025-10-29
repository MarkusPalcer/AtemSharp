namespace AtemSharp.State.Streaming;

public class StreamingState
{
    public string ServiceName { get; internal set; } = string.Empty;
    public string Url { get; internal set; } = string.Empty;
    public string Key { get; internal set; } = string.Empty;

    public Bitrates VideoBitrates { get; } = new();
    public Bitrates AudioBitrates { get; } = new();

    public StreamingStatus Status { get; internal set; } = StreamingStatus.Idle;

    public StreamingError Error { get; internal set; } = StreamingError.None;
    public Duration Duration { get; } = new();
    public uint EncodingBitrate { get; set; }
    public ushort CacheUsed { get; set; }
}
