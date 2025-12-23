using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

/// <summary>
/// Used to control how the mediaplayer plays a clip
/// </summary>
[Command("SCPS")]
[BufferSize(8)]
public partial class MediaPlayerStatusCommand(MediaPlayer mediaPlayer) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _id = mediaPlayer.Id;
    [SerializedField(2, 0)] private bool _play = mediaPlayer.IsPlaying;
    [SerializedField(3, 1)] private bool _loop = mediaPlayer.IsLooping;
    [SerializedField(4, 2)] private bool _atBeginning = mediaPlayer.IsAtBeginning;
    [SerializedField(6, 3)] private ushort _clipFrame = mediaPlayer.ClipFrame;
}
