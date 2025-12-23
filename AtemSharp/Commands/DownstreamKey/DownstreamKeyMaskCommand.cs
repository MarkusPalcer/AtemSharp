using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Used to update downstream keyer mask properties
/// </summary>
[Command("CDsM")]
[BufferSize(12)]
public partial class DownstreamKeyMaskCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _downstreamKeyerId = dsk.Id;
    [SerializedField(2, 0)] private bool _enabled = dsk.Properties.Mask.Enabled;

    [SerializedField(4, 1)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _top = dsk.Properties.Mask.Top;

    [SerializedField(6, 2)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _bottom = dsk.Properties.Mask.Bottom;

    [SerializedField(8, 3)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _left = dsk.Properties.Mask.Left;

    [SerializedField(10, 4)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _right = dsk.Properties.Mask.Right;
}
