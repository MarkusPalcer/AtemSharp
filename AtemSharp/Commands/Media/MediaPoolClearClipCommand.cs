using AtemSharp.State.Media;

namespace AtemSharp.Commands.Media;

[Command("CMPC")]
[BufferSize(4)]
public partial class MediaPoolClearClipCommand(MediaPoolEntry entry) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _index = (byte)entry.Id;
}
