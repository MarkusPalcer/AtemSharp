using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("CSTL")]
[BufferSize(4)]
// TODO: How to determine whether the entry is really a clip not a still?
public partial class MediaPoolClearStillCommand(MediaPoolEntry entry): SerializedCommand
{
    [SerializedField(0)] [NoProperty] private byte _index = entry.Id;
}
