using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to update downstream keyer general properties (pre-multiply, clip, gain, invert)
/// </summary>
[Command("CDsG")]
[BufferSize(12)]
public partial class DownstreamKeyGeneralCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _downstreamKeyerId = dsk.Id;

    /// <summary>
    /// Whether to pre-multiply the key signal
    /// </summary>
    [SerializedField(2, 0)] private bool _preMultiply = dsk.Properties.PreMultiply;

    /// <summary>
    /// Clip threshold
    /// </summary>
    [SerializedField(4, 1)] [SerializedType(typeof(short))] [ScalingFactor(10.0)]
    private double _clip = dsk.Properties.Clip;

    /// <summary>
    /// Gain value
    /// </summary>
    [SerializedField(6, 2)] [SerializedType(typeof(short))] [ScalingFactor(10.0)]
    private double _gain = dsk.Properties.Gain;

    /// <summary>
    /// Whether to invert the key signal
    /// </summary>
    [SerializedField(8, 3)] private bool _invert = dsk.Properties.Invert;
}
