namespace AtemSharp.State;

/// <summary>
/// Video state container for ATEM devices
/// </summary>
public class VideoState
{
    /// <summary>
    /// Downstream keyers available on this device
    /// </summary>
    public DownstreamKeyer?[] DownstreamKeyers { get; set; } = [];
}