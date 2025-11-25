using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to update downstream keyer mask properties
/// </summary>
[Command("CDsM")]
[BufferSize(12)]
public partial class DownstreamKeyMaskCommand : SerializedCommand
{
    [SerializedField(1)]
    [NoProperty]
    internal readonly byte DownstreamKeyerId;

    /// <summary>
    /// Whether the mask is enabled
    /// </summary>
    [SerializedField(2,0)]
    private bool _enabled;

    /// <summary>
    /// Top edge of the mask
    /// </summary>
    [SerializedField(4,1)]
    [SerializedType(typeof(short))]
    [ScalingFactor(1000.0)]
    private double _top;

    /// <summary>
    /// Bottom edge of the mask
    /// </summary>
    [SerializedField(6,2)]
    [SerializedType(typeof(short))]
    [ScalingFactor(1000.0)]
    private double _bottom;

    /// <summary>
    /// Left edge of the mask
    /// </summary>
    [SerializedField(8,3)]
    [SerializedType(typeof(short))]
    [ScalingFactor(1000.0)]
    private double _left;

    /// <summary>
    /// Right edge of the mask
    /// </summary>
    [SerializedField(10,4)]
    [SerializedType(typeof(short))]
    [ScalingFactor(1000.0)]
    private double _right;

    public DownstreamKeyMaskCommand(DownstreamKeyer dsk)
    {
        DownstreamKeyerId = dsk.Id;
        var maskProps = dsk.Properties.Mask;

        // Initialize from current state (direct field access = no flags set)
        _enabled = maskProps.Enabled;
        _top = maskProps.Top;
        _bottom = maskProps.Bottom;
        _left = maskProps.Left;
        _right = maskProps.Right;
    }
}
