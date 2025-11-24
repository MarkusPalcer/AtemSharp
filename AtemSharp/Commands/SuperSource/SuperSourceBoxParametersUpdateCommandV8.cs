using System.Drawing;
using AtemSharp.Enums;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.SuperSource;

[Command("SSBP", ProtocolVersion.V8_0)]
public partial class SuperSourceBoxParametersUpdateCommandV8 : IDeserializedCommand
{
    [DeserializedField(0)]  private byte _superSourceId;
    [DeserializedField(1)]  private byte _boxId;
    [DeserializedField(2)]  private bool _enabled;
    [DeserializedField(4)]  private ushort _source;
    [DeserializedField(6)]  [ScalingFactor(100)] [SerializedType(typeof(short))] private double _x;
    [DeserializedField(8)]  [ScalingFactor(100)] [SerializedType(typeof(short))] private double _y;
    [DeserializedField(10)] [ScalingFactor(1000)] [SerializedType(typeof(ushort))] private double _size;
    [DeserializedField(12)] private bool _cropped;
    [DeserializedField(14)] [ScalingFactor(1000)] [SerializedType(typeof(short))] private double _cropTop;
    [DeserializedField(16)] [ScalingFactor(1000)] [SerializedType(typeof(short))] private double _cropBottom;
    [DeserializedField(18)] [ScalingFactor(1000)] [SerializedType(typeof(short))] private double _cropLeft;
    [DeserializedField(20)] [ScalingFactor(1000)] [SerializedType(typeof(short))] private double _cropRight;

    public void ApplyToState(AtemState state)
    {
        var box = state.Video.SuperSources[_superSourceId].Boxes.GetOrCreate(_boxId);
        box.Id = _boxId;
        box.Enabled = _enabled;
        box.Source = _source;
        box.Location = new PointF((float)_x, (float)_y);
        box.Size = _size;
        box.Cropped = _cropped;
        box.CropTop = _cropTop;
        box.CropBottom = _cropBottom;
        box.CropLeft =  _cropLeft;
        box.CropRight = _cropRight;
    }
}
