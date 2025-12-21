using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// SuperSource configuration and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SuperSourceInfo
{
    public byte Id { get; internal init; }

    /// <summary>
    /// Number of SuperSource boxes available
    /// </summary>
    public byte BoxCount { get; internal set; }
}
