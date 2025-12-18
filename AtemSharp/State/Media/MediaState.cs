using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Media;

public class MediaState
{
    internal MediaState()
    {
        Players = new ItemCollection<byte, MediaPlayer>(id => new MediaPlayer { Id = id });
        Clips = new ItemCollection<ushort, MediaPoolEntry>(id => new MediaPoolEntry { Id = id });
        Frames = new ItemCollection<ushort, MediaPoolEntry>(id => new MediaPoolEntry { Id = id });
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, MediaPoolEntry> Frames { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, MediaPoolEntry> Clips { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, MediaPlayer> Players { get; }
}
