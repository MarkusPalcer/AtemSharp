using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

/// <summary>
/// Used to switch the media player to a certain still or clip
/// </summary>
[Command("MPSS")]
[BufferSize(8)]
public partial class MediaPlayerSourceCommand(MediaPlayer player) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _mediaPlayerId = player.Id;

    [SerializedField(2, 0)] private MediaSourceType _sourceType = player.SourceType;

    [SerializedField(3, 1)] private byte _stillIndex = player.StillIndex;

    [SerializedField(4, 2)] private byte _clipIndex = player.ClipIndex;
}
