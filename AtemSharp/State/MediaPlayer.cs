using AtemSharp.Enums;

namespace AtemSharp.State;

public class MediaPlayer
{
    public byte Id { get; set; }
    public MediaSourceType SourceType { get; set; }
    public byte StillIndex { get; set; }
    public byte ClipIndex { get; set; }
}
