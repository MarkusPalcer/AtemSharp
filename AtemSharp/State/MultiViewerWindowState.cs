namespace AtemSharp.State;

/// <summary>
/// Represents the full state of a MultiViewer window, extending the source configuration with display options
/// </summary>
public class MultiViewerWindowState : MultiViewerSourceState
{
    /// <summary>
    /// Whether safe title overlay is enabled for this window (optional)
    /// </summary>
    public bool? SafeTitle { get; set; }

    /// <summary>
    /// Whether audio meter overlay is enabled for this window (optional)
    /// </summary>
    public bool? AudioMeter { get; set; }
}