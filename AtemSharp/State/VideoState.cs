namespace AtemSharp.State;

/// <summary>
/// Video state container for ATEM devices
/// </summary>
public class VideoState
{
    /// <summary>
    /// Input channels available on this device
    /// </summary>
    public Dictionary<int, InputChannel> Inputs { get; set; } = [];

    /// <summary>
    /// Mix effects available on this device
    /// </summary>
    public Dictionary<int, MixEffect> MixEffects { get; set; } = [];

    /// <summary>
    /// Downstream keyers available on this device
    /// </summary>
    public Dictionary<int, DownstreamKeyer> DownstreamKeyers { get; set; } = [];

    /// <summary>
    /// Auxiliary output sources
    /// </summary>
    public Dictionary<int, int> Auxiliaries { get; set; } = [];
}