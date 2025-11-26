using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

[Command("CSTL")]
[BufferSize(4)]
public partial class MediaPoolClearStillCommand(MediaPoolEntry entry) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _index = entry.Id;
}
