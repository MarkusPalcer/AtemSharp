namespace AtemSharp.State;

/// <summary>
/// Represents the source configuration for a MultiViewer window
/// </summary>
public class MultiViewerSourceState
{
    /// <summary>
    /// The video source assigned to this window (input number)
    /// </summary>
    public int Source { get; set; }

    /// <summary>
    /// The window index within the MultiViewer (read-only in TypeScript, but settable here for C# flexibility)
    /// </summary>
    public int WindowIndex { get; set; }

    /// <summary>
    /// Whether this window supports VU meter display
    /// </summary>
    public bool SupportsVuMeter { get; set; }

    /// <summary>
    /// Whether this window supports safe area overlay
    /// </summary>
    public bool SupportsSafeArea { get; set; }
}