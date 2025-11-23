using AtemSharp.Commands;
using AtemSharp.Helpers;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Media;

[Command("SMPC")]
[BufferSize(68)]
public partial class MediaPoolSetClipCommand(MediaPoolEntry entry) : SerializedCommand
{
    [SerializedField(1)] private byte _id = entry.Id;
    [CustomSerialization] private string _name = entry.Name;
    [SerializedField(66)] private ushort _frames = entry.FrameCount;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt8(3, 0);
        buffer.WriteString(_name, 2, 44);
    }
}
