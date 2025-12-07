using System.Drawing;
using AtemSharp.State;

namespace AtemSharp.Commands.SuperSource;

[Command("SSBP")]
public partial class SuperSourceBoxParametersUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _boxId;
    [DeserializedField(1)] private bool _enabled;
    [DeserializedField(2)] private ushort _source;

    [DeserializedField(4)] [ScalingFactor(100)] [SerializedType(typeof(short))]
    private double _x;

    [DeserializedField(6)] [ScalingFactor(100)] [SerializedType(typeof(short))]
    private double _y;

    [DeserializedField(8)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))]
    private double _size;

    [DeserializedField(10)] private bool _cropped;

    [DeserializedField(12)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _cropTop;

    [DeserializedField(14)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _cropBottom;

    [DeserializedField(16)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _cropLeft;

    [DeserializedField(18)] [ScalingFactor(1000)] [SerializedType(typeof(short))]
    private double _cropRight;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var box = state.Video.SuperSources[0].Boxes.GetOrCreate(_boxId);
        box.SuperSourceId = 0;
        box.Enabled = _enabled;
        box.Source = _source;
        box.Location = new PointF((float)_x, (float)_y);
        box.Size = _size;
        box.Cropped = _cropped;
        box.CropTop = _cropTop;
        box.CropBottom = _cropBottom;
        box.CropLeft = _cropLeft;
        box.CropRight = _cropRight;
    }
}
