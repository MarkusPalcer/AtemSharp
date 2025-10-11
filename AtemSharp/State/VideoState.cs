namespace AtemSharp.State;

/// <summary>
/// Video state container for ATEM devices
/// </summary>
public class VideoState
{
    /// <summary>
    /// Mix effects available on this device
    /// </summary>
    public MixEffect?[] MixEffects { get; set; } = [];

    /// <summary>
    /// Downstream keyers available on this device
    /// </summary>
    public DownstreamKeyer?[] DownstreamKeyers { get; set; } = [];
}