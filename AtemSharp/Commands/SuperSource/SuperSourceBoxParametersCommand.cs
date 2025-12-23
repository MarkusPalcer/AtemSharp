using System.Drawing;
using AtemSharp.State.Info;
using AtemSharp.State.Video.SuperSource;

namespace AtemSharp.Commands.SuperSource;

/// <summary>
/// Used to set the parameters of a super source box
/// </summary>
/// <remarks>
/// Used for <see cref="ProtocolVersion"/> below 8.0
/// </remarks>
[Command("CSBP")]
[BufferSize(24)]
public partial class SuperSourceBoxParametersCommand(SuperSourceBox box) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly byte _boxId = box.Id;

    [SerializedField(3, 0)] private bool _enabled = box.Enabled;
    [SerializedField(4, 1)] private ushort _source = box.Source;

    [SerializedField(6, 2)] [ScalingFactor(100)] [SerializedType(typeof(short))]
    private double _x = box.Location.X;

    [SerializedField(8, 3)] [ScalingFactor(100)] [SerializedType(typeof(short))]
    private double _y = box.Location.Y;

    [SerializedField(10, 4)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _size = box.Size;

    [SerializedField(12, 5)] private bool _cropped = box.Cropped;

    [SerializedField(14, 6)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _cropTop = box.CropTop;

    [SerializedField(16, 7)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _cropBottom = box.CropBottom;

    [SerializedField(18, 8)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _cropLeft = box.CropLeft;

    [SerializedField(20, 9)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
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
