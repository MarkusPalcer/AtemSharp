using AtemSharp.Enums;

namespace AtemSharp.State;

public class MediaPlayer : ArrayItem
{
    internal override void SetId(int id) => Id = (byte)id;

    public byte Id { get; internal set; }
    public MediaSourceType SourceType { get; internal set; }
    public byte StillIndex { get; internal set; }
    public byte ClipIndex { get; internal set; }
    public bool IsPlaying { get; internal set; }
    public bool IsLooping { get; internal set; }
    public bool IsAtBeginning { get; internal set; }
    public ushort ClipFrame { get; internal set; }
}
