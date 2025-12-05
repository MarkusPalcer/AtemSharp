using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Media;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MediaPoolEntry : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;

    public byte Id { get; internal set; }
    public bool IsUsed { get; internal set; }

    public string Name { get; internal set; } = string.Empty;

    public string Hash { get; internal set; } = string.Empty;
    public string FileName { get; internal set; } = string.Empty;
    public ushort FrameCount { get; internal set; }
}
