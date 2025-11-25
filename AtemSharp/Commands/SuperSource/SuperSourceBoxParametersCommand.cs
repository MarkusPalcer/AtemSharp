using AtemSharp.Lib;
using AtemSharp.State.Info;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Commands.SuperSource;

[Command("CSBP")]
[BufferSize(24)]
public partial class SuperSourceBoxParametersCommand(SuperSourceBox box) : SerializedCommand
{
    private readonly byte _superSourceId = box.SuperSourceId;
    private readonly byte _boxId = box.Id;

    // Custom serialization because of version differences
    // Automatic class selection based on version only possible for deserialized commands
    [CustomSerialization(0)] private bool _enabled = box.Enabled;
    [CustomSerialization(1)] private ushort _source = box.Source;
    [CustomSerialization(2)] private double _x = box.Location.X;
    [CustomSerialization(3)] private double _y = box.Location.Y;
    [CustomSerialization(4)] private double _size = box.Size;
    [CustomSerialization(5)] private bool _cropped = box.Cropped;
    [CustomSerialization(6)] private double _cropTop = box.CropTop;
    [CustomSerialization(7)] private double _cropBottom = box.CropBottom;
    [CustomSerialization(8)] private double _cropLeft = box.CropLeft;
    [CustomSerialization(9)] private double _cropRight = box.CropRight;

    private void SerializeInternal(byte[] buffer, ProtocolVersion version)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);

        var i = 0;
        if (version >= ProtocolVersion.V8_0)
        {
            i = 1;
            buffer.WriteUInt8(_superSourceId, i + 1);
        }

        buffer.WriteUInt8(_boxId, i + 2);
        buffer.WriteBoolean(_enabled, i + 3);

        if (i == 1) i++; // Needs to be 2 byte aligned now

        buffer.WriteUInt16BigEndian(_source, i + 4);
        buffer.WriteInt16BigEndian((short)(_x * 100.0), i + 6);
        buffer.WriteInt16BigEndian((short)(_y * 100.0), i + 8);
        buffer.WriteUInt16BigEndian((ushort)(_size * 1000.0), i + 10);
        buffer.WriteBoolean(_cropped, i + 12);
        buffer.WriteUInt16BigEndian((ushort)(_cropTop * 1000.0), i + 14);
        buffer.WriteUInt16BigEndian((ushort)(_cropBottom * 1000.0), i + 16);
        buffer.WriteUInt16BigEndian((ushort)(_cropLeft * 1000.0), i + 18);
        buffer.WriteUInt16BigEndian((ushort)(_cropRight * 1000.0), i + 20);
    }
}
