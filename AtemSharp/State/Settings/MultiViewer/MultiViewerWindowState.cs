using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Settings.MultiViewer;

/// <summary>
/// Represents the full state of a MultiViewer window, extending the source configuration with display options
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MultiViewerWindowState : ItemWithId<byte>
{
    public byte MultiViewerId { get; internal set; }

    /// <summary>
    /// Whether safe title overlay is enabled for this window (optional)
    /// </summary>
    public bool SafeTitle { get; internal set; }

    /// <summary>
    /// Whether audio meter overlay is enabled for this window (optional)
    /// </summary>
    public bool AudioMeter { get; internal set; }

    /// <summary>
    /// The video source assigned to this window (input number)
    /// </summary>
    public ushort Source { get; internal set; }

    /// <summary>
    /// The window index within the MultiViewer (read-only in TypeScript, but settable here for C# flexibility)
    /// </summary>
    public byte WindowIndex { get; internal set; }

    /// <summary>
    /// Whether this window supports VU meter display
    /// </summary>
    public bool SupportsVuMeter { get; internal set; }

    /// <summary>
    /// Whether this window supports safe area overlay
    /// </summary>
    public bool SupportsSafeArea { get; internal set; }

    internal override void SetId(byte id) => WindowIndex = id;
}
