using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

[Command("CMPC")]
[BufferSize(4)]
// TODO: How to determine whether the entry is really a clip not a still?
public partial class MediaPoolClearClipCommand(MediaPoolEntry entry): SerializedCommand
{
    [SerializedField(0)] [NoProperty] private byte _index = entry.Id;
}
