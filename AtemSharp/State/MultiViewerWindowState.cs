namespace AtemSharp.State;

/// <summary>
/// Represents the full state of a MultiViewer window, extending the source configuration with display options
/// </summary>
public class MultiViewerWindowState
{
    public byte MultiViewerId { get; internal set; }

    /// <summary>
    /// Whether safe title overlay is enabled for this window (optional)
    /// </summary>
    public bool SafeTitle { get; set; }

    /// <summary>
    /// Whether audio meter overlay is enabled for this window (optional)
    /// </summary>
    // TODO: Unify names - Vu vs AudioMeter
    public bool AudioMeter { get; set; }

    /// <summary>
    /// The video source assigned to this window (input number)
    /// </summary>
    public ushort Source { get; set; }

    /// <summary>
    /// The window index within the MultiViewer (read-only in TypeScript, but settable here for C# flexibility)
    /// </summary>
    public byte WindowIndex { get; set; }

    /// <summary>
    /// Whether this window supports VU meter display
    /// </summary>
    public bool SupportsVuMeter { get; set; }

    /// <summary>
    /// Whether this window supports safe area overlay
    /// </summary>
    public bool SupportsSafeArea { get; set; }
}
