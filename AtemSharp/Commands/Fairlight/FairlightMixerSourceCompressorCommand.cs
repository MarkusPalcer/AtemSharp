using AtemSharp.Helpers;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CICP")]
[BufferSize(40)]
public partial class FairlightMixerSourceCompressorCommand(Source source) : SerializedCommand
{
    [SerializedField(2)]
    [NoProperty]
    private readonly ushort _inputId = source.InputId;

    [SerializedField(8)]
    [NoProperty]
    private readonly long _sourceId = source.Id;

    [SerializedField(16)]
    private bool _compressorEnabled = source.Dynamics.Compressor.Enabled;

    [SerializedField(20)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _threshold = source.Dynamics.Compressor.Threshold;

    [SerializedField(24)]
    [SerializedType(typeof(short))]
    [ScalingFactor(100.0)]
    private double _ratio = source.Dynamics.Compressor.Ratio;

    [SerializedField(28)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _attack = source.Dynamics.Compressor.Attack;

    [SerializedField(32)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _hold = source.Dynamics.Compressor.Hold;


    [SerializedField(36)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _release = source.Dynamics.Compressor.Release;
}
