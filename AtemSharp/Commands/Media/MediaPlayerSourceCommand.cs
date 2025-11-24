using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("MPSS")]
[BufferSize(8)]
public partial class MediaPlayerSourceCommand(MediaPlayer player) : SerializedCommand
{
    [SerializedField(2,0)]
    private MediaSourceType _sourceType = player.SourceType;

    [SerializedField(3,1)]
    private byte _stillIndex = player.StillIndex;

    [SerializedField(4,2)]
    private byte _clipIndex = player.ClipIndex;

    [SerializedField(1)]
    [NoProperty]
    private readonly byte _mediaPlayerId = player.Id;
}
