using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Mix effect configuration and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MixEffectInfo
{
    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public byte Id { get; internal init; }

    /// <summary>
    /// Number of keyers available in this mix effect
    /// </summary>
    public byte KeyCount { get; internal set; }
}
