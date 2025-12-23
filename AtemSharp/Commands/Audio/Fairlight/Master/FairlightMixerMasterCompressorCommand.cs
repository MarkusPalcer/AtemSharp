using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

/// <summary>
/// Used to configure the compressor on the master channel of the fairlight mixer
/// </summary>
[Command("CMCP")]
[BufferSize(24)]
public partial class FairlightMixerMasterCompressorCommand(MasterProperties master) : SerializedCommand
{
    [SerializedField(1, 0)]
    private bool _enabled = master.Dynamics.Compressor.Enabled;

    [SerializedField(20)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _release = master.Dynamics.Compressor.Release;

    [SerializedField(16)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _hold = master.Dynamics.Compressor.Hold;

    [SerializedField(12)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _attack = master.Dynamics.Compressor.Attack;

    [SerializedField(8)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(short))]
    private double _ratio = master.Dynamics.Compressor.Ratio;

    [SerializedField(4)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _threshold = master.Dynamics.Compressor.Threshold;
}
