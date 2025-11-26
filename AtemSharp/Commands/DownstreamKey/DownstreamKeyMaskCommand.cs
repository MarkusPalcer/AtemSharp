using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to update downstream keyer mask properties
/// </summary>
[Command("CDsM")]
[BufferSize(12)]
public partial class DownstreamKeyMaskCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _downstreamKeyerId = dsk.Id;

    /// <summary>
    /// Whether the mask is enabled
    /// </summary>
    [SerializedField(2, 0)] private bool _enabled = dsk.Properties.Mask.Enabled;

    /// <summary>
    /// Top edge of the mask
    /// </summary>
    [SerializedField(4, 1)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _top = dsk.Properties.Mask.Top;

    /// <summary>
    /// Bottom edge of the mask
    /// </summary>
    [SerializedField(6, 2)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _bottom = dsk.Properties.Mask.Bottom;

    /// <summary>
    /// Left edge of the mask
    /// </summary>
    [SerializedField(8, 3)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _left = dsk.Properties.Mask.Left;

    /// <summary>
    /// Right edge of the mask
    /// </summary>
    [SerializedField(10, 4)] [SerializedType(typeof(short))] [ScalingFactor(1000.0)]
    private double _right = dsk.Properties.Mask.Right;
}
