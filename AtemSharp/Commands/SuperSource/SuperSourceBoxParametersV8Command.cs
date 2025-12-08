using System.Drawing;
using AtemSharp.State.Info;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Commands.SuperSource;

[Command("CSBP", ProtocolVersion.V8_0)]
[BufferSize(24)]
public partial class SuperSourceBoxParametersV8Command(SuperSourceBox box) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _superSourceId = box.SuperSourceId;
    [SerializedField(3)] [NoProperty] private readonly byte _boxId = box.Id;

    [SerializedField(4, 0)] private bool _enabled = box.Enabled;
    [SerializedField(6, 1)] private ushort _source = box.Source;

    [SerializedField(8, 2)] [ScalingFactor(100)] [SerializedType(typeof(short))]
    private double _x = box.Location.X;

    [SerializedField(10, 3)] [ScalingFactor(100)] [SerializedType(typeof(short))]
    private double _y = box.Location.Y;

    [SerializedField(12, 4)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _size = box.Size;

    [SerializedField(14, 5)] private bool _cropped = box.Cropped;

    [SerializedField(16, 6)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _cropTop = box.CropTop;

    [SerializedField(18, 7)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _cropBottom = box.CropBottom;

    [SerializedField(20, 8)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _cropLeft = box.CropLeft;

    [SerializedField(22, 9)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _cropRight = box.CropRight;

    public PointF Location
    {
        get => new((float)_x, (float)_y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
