using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Multiviewer configuration and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MultiViewerInfo
{
    /// <summary>
    /// Number of multiviewers available
    /// </summary>
    public int Count { get; internal set; } = -1;

    /// <summary>
    /// Number of windows per multiviewer
    /// </summary>
    public int WindowCount { get; internal set; } = -1;
}
