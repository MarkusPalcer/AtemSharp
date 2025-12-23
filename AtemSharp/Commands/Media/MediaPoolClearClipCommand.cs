using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

/// <summary>
/// Used to delete a clip from the media pool
/// </summary>
[Command("CMPC")]
[BufferSize(4)]
public partial class MediaPoolClearClipCommand(Clip entry) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _index = (byte)entry.Id;
}
