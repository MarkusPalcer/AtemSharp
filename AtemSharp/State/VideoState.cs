namespace AtemSharp.State;

/// <summary>
/// Video state container for ATEM devices
/// </summary>
public class VideoState
{
    /// <summary>
    /// Input channels available on this device
    /// </summary>
    public Dictionary<ushort, InputChannel> Inputs { get; } = [];

    /// <summary>
    /// Mix effects available on this device
    /// </summary>
    public MixEffect[] MixEffects { get; internal set; } = [];

    /// <summary>
    /// Downstream keyers available on this device
    /// </summary>
    public DownstreamKeyer[] DownstreamKeyers { get; internal set; } = [];

    /// <summary>
    /// Auxiliary output sources
    /// </summary>
    public AuxiliaryOutput[] Auxiliaries { get; internal set; } = [];

    public SuperSource[] SuperSources { get; internal set; } = [];
}
