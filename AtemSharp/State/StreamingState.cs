namespace AtemSharp.State;

public class StreamingState
{
    public string ServiceName { get; internal set; } = string.Empty;
    public string Url { get; internal set; } = string.Empty;
    public string Key { get; internal set; } = string.Empty;
    public uint VideoBitrate1 { get; internal set; }
    public uint VideoBitrate2 { get; internal set; }
    public uint AudioBitrateLow { get; internal set; } = 128000;
    public uint AudioBitrateHigh { get; internal set; } = 192000;
}
