namespace AtemSharp.State.Video;

/// <summary>
/// Video state container for ATEM devices
/// </summary>
public class VideoState
{
    /// <summary>
    /// Input channels available on this device
    /// </summary>
    public Dictionary<ushort, InputChannel.InputChannel> Inputs { get; } = [];

    /// <summary>
    /// Mix effects available on this device
    /// </summary>
    public MixEffect.MixEffect[] MixEffects { get; internal set; } = [];

    /// <summary>
    /// Downstream keyers available on this device
    /// </summary>
    public DownstreamKeyer.DownstreamKeyer[] DownstreamKeyers { get; internal set; } = [];

    /// <summary>
    /// Auxiliary output sources
    /// </summary>
    public AuxiliaryOutput[] Auxiliaries { get; internal set; } = [];

    public SuperSource.SuperSource[] SuperSources { get; internal set; } = [];
}
