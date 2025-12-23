using System.Diagnostics.CodeAnalysis;
using AtemSharp.Types;

namespace AtemSharp.State.Media;

public class MediaState
{
    internal MediaState()
    {
        Players = new ItemCollection<byte, MediaPlayer>(id => new MediaPlayer { Id = id });
        Clips = new ItemCollection<ushort, Clip>(id => new Clip { Id = id });
        Frames = new ItemCollection<ushort, Still>(id => new Still { Id = id });
    }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, Still> Frames { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<ushort, Clip> Clips { get; }

    [ExcludeFromCodeCoverage(Justification = "Auto-Properties aren't tested")]
    public ItemCollection<byte, MediaPlayer> Players { get; }
}
