using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// Mix effect configuration and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MixEffectInfo : ItemWithId<int>
{
    internal override void SetId(int id) => Id = (byte)id;

    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public byte Id { get; internal set; }

    /// <summary>
    /// Number of keyers available in this mix effect
    /// </summary>
    public byte KeyCount { get; internal set; }
}
