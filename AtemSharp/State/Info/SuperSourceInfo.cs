using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Info;

/// <summary>
/// SuperSource configuration and capabilities
/// </summary>
[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class SuperSourceInfo : ItemWithId<int>
{
    internal override void SetId(int id) => Id = (byte)id;
    public byte Id { get; internal set; }

    /// <summary>
    /// Number of SuperSource boxes available
    /// </summary>
    public byte BoxCount { get; internal set; }
}
