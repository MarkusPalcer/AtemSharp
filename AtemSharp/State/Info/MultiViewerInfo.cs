namespace AtemSharp.State.Info;

/// <summary>
/// Multiviewer configuration and capabilities
/// </summary>
public class MultiViewerInfo
{
    /// <summary>
    /// Number of multiviewers available
    /// </summary>
    public int Count { get; set; } = -1;

    /// <summary>
    /// Number of windows per multiviewer
    /// </summary>
    public int WindowCount { get; set; } = -1;
}
