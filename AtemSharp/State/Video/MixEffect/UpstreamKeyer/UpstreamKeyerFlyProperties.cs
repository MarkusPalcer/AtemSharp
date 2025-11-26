namespace AtemSharp.State.Video.MixEffect.UpstreamKeyer;

/// <summary>
/// Fly properties for upstream keyer
/// </summary>
public class UpstreamKeyerFlyProperties
{
    /// <summary>
    /// Whether key frame A is set
    /// </summary>
    public bool IsASet { get; internal set; }

    /// <summary>
    /// Whether key frame B is set
    /// </summary>
    public bool IsBSet { get; internal set; }

    /// <summary>
    /// Current key frame state flags
    /// </summary>
    public IsAtKeyFrame IsAtKeyFrame { get; internal set; }

    /// <summary>
    /// Run to infinite index
    /// </summary>
    public int RunToInfiniteIndex { get; internal set; }
}
