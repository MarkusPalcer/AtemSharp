using AtemSharp.Enums;

namespace AtemSharp.State;

public class MediaPlayer
{
    public byte Id { get; set; }
    public MediaSourceType SourceType { get; set; }
    public byte StillIndex { get; set; }
    public byte ClipIndex { get; set; }
    public bool IsPlaying { get; set; }
    public bool IsLooping { get; set; }
    public bool IsAtBeginning { get; set; }
    public ushort ClipFrame { get; set; }
}
