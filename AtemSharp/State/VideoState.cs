namespace AtemSharp.State;

/// <summary>
/// Video state container for ATEM devices
/// </summary>
public class VideoState
{
    /// <summary>
    /// Input channels available on this device
    /// </summary>
    public Dictionary<ushort, InputChannel> Inputs { get; set; } = [];

    /// <summary>
    /// Mix effects available on this device
    /// </summary>
    // TODO: Convert to Array and instantiate with size from Topology command
    public Dictionary<int, MixEffect> MixEffects { get; set; } = [];

    /// <summary>
    /// Downstream keyers available on this device
    /// </summary>
    public DownstreamKeyer[] DownstreamKeyers { get; set; } = [];

    /// <summary>
    /// Auxiliary output sources
    /// </summary>
    // TODO: Check if it can be converted to an array
    // TODO: Introduce type for auxiliary output and pass this to AuxSourceCommand instead of index
    public Dictionary<byte, ushort> Auxiliaries { get; set; } = [];

    public SuperSource[] SuperSources { get; internal set; } = [];
}
