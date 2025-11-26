using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("CMLP")]
[BufferSize(20)]
public partial class FairlightMixerMasterLimiterCommand(MasterProperties master) : SerializedCommand
{
    [SerializedField(1, 0)] private bool _limiterEnabled = master.Dynamics.Limiter.Enabled;

    [SerializedField(4, 1)] [SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _threshold = master.Dynamics.Limiter.Threshold;

    [SerializedField(8, 2)] [SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _attack = master.Dynamics.Limiter.Attack;

    [SerializedField(12, 3)] [SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _hold = master.Dynamics.Limiter.Hold;

    [SerializedField(16, 4)] [SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _release = master.Dynamics.Limiter.Release;
}
