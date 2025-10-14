using AtemSharp.Enums;

namespace AtemSharp.State;

/// <summary>
/// Fly properties for upstream keyer
/// </summary>
public class UpstreamKeyerFlyProperties
{
    /// <summary>
    /// Whether key frame A is set
    /// </summary>
    public bool IsASet { get; set; }
    
    /// <summary>
    /// Whether key frame B is set
    /// </summary>
    public bool IsBSet { get; set; }
    
    /// <summary>
    /// Current key frame state flags
    /// </summary>
    public IsAtKeyFrame IsAtKeyFrame { get; set; }
    
    /// <summary>
    /// Run to infinite index
    /// </summary>
    public int RunToInfiniteIndex { get; set; }
}