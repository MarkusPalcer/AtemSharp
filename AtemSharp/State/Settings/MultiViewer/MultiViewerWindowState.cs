using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Settings.MultiViewer;

/// <summary>
/// Represents the full state of a MultiViewer window, extending the source configuration with display options
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
[DebuggerDisplay("{" + nameof(ToString) + ",nq}")]
public class MultiViewerWindowState
{
    public byte MultiViewerId { get; internal init; }

    /// <summary>
    /// Whether safe title overlay is enabled for this window (optional)
    /// </summary>
    public bool SafeTitle { get; internal set; }

    /// <summary>
    /// Whether audio meter overlay is enabled for this window (optional)
    /// </summary>
    public bool VuMeter { get; internal set; }

    /// <summary>
    /// The video source assigned to this window (input number)
    /// </summary>
    public ushort Source { get; internal set; }

    /// <summary>
    /// The window index within the MultiViewer (read-only in TypeScript, but settable here for C# flexibility)
    /// </summary>
    public byte WindowIndex { get; internal init; }

    /// <summary>
    /// Whether this window supports VU meter display
    /// </summary>
    public bool SupportsVuMeter { get; internal set; }

    /// <summary>
    /// Whether this window supports safe area overlay
    /// </summary>
    public bool SupportsSafeArea { get; internal set; }

    [ExcludeFromCodeCoverage]
    public override string ToString() => $"{GetType().Name} #{MultiViewerId}.#{WindowIndex}";
}
