using System.Diagnostics.CodeAnalysis;

namespace AtemSharp.State.Media;

[ExcludeFromCodeCoverage(Justification="Auto-Properties aren't tested")]
public class MediaPlayer
{
    public byte Id { get; internal init; }
    public MediaSourceType SourceType { get; internal set; }
    public byte StillIndex { get; internal set; }
    public byte ClipIndex { get; internal set; }
    public bool IsPlaying { get; internal set; }
    public bool IsLooping { get; internal set; }
    public bool IsAtBeginning { get; internal set; }
    public ushort ClipFrame { get; internal set; }
}
