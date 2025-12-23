using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

/// <summary>
/// Used to remove a still from the media pool
/// </summary>
[Command("CSTL")]
[BufferSize(4)]
public partial class MediaPoolClearStillCommand(MediaPoolEntry entry) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _index = (byte)entry.Id;
}
