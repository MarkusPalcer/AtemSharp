namespace AtemSharp.State.Info;

/// <summary>
/// SuperSource configuration and capabilities
/// </summary>
public class SuperSourceInfo : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;
    public byte Id { get; internal set; }

    /// <summary>
    /// Number of SuperSource boxes available
    /// </summary>
    public byte BoxCount { get; internal set; }
}
