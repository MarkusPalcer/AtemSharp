using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Used to update downstream keyer general properties (pre-multiply, clip, gain, invert)
/// </summary>
[Command("CDsG")]
[BufferSize(12)]
public partial class DownstreamKeyGeneralCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _downstreamKeyerId = dsk.Id;
    [SerializedField(2, 0)] private bool _preMultiply = dsk.Properties.PreMultiply;

    [SerializedField(4, 1)] [SerializedType(typeof(short))] [ScalingFactor(10.0)]
    private double _clip = dsk.Properties.Clip;

    [SerializedField(6, 2)] [SerializedType(typeof(short))] [ScalingFactor(10.0)]
    private double _gain = dsk.Properties.Gain;

    [SerializedField(8, 3)] private bool _invert = dsk.Properties.Invert;
}
