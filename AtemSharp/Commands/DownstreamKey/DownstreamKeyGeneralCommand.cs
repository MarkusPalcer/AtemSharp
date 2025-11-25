using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to update downstream keyer general properties (pre-multiply, clip, gain, invert)
/// </summary>
[Command("CDsG")]
[BufferSize(12)]
public partial class DownstreamKeyGeneralCommand : SerializedCommand
{
    [SerializedField(1)]
    [NoProperty]
    internal readonly byte DownstreamKeyerId;

    /// <summary>
    /// Whether to pre-multiply the key signal
    /// </summary>
    [SerializedField(2,0)]
    private bool _preMultiply;

    /// <summary>
    /// Clip threshold
    /// </summary>
    [SerializedField(4,1)]
    [SerializedType(typeof(short))]
    [ScalingFactor(10.0)]
    private double _clip;

    /// <summary>
    /// Gain value
    /// </summary>
    [SerializedField(6,2)]
    [SerializedType(typeof(short))]
    [ScalingFactor(10.0)]
    private double _gain;

    /// <summary>
    /// Whether to invert the key signal
    /// </summary>
    [SerializedField(8,3)]
    private bool _invert;


    public DownstreamKeyGeneralCommand(DownstreamKeyer dsk)
    {
        DownstreamKeyerId = dsk.Id;
        var dskProps = dsk.Properties;

        // Initialize from current state (direct field access = no flags set)
        _preMultiply = dskProps.PreMultiply;
        _clip = dskProps.Clip;
        _gain = dskProps.Gain;
        _invert = dskProps.Invert;
    }
}
