namespace AtemSharp.Commands.Audio.Fairlight.Source;

[Command("CILP")]
[BufferSize(36)]
public partial class FairlightMixerSourceLimiterCommand(State.Audio.Fairlight.Source source) : SerializedCommand
{
    [SerializedField(2)] [NoProperty] private readonly ushort _inputId = source.InputId;

    [SerializedField(8)] [NoProperty] private readonly long _sourceId = source.Id;

    [SerializedField(16, 0)] private bool _limiterEnabled = source.Dynamics.Limiter.Enabled;

    [SerializedField(20, 1)] [ScalingFactor(100)] [SerializedType(typeof(int))]
    private double _threshold = source.Dynamics.Limiter.Threshold;

    [SerializedField(24, 2)] [ScalingFactor(100)] [SerializedType(typeof(int))]
    private double _attack = source.Dynamics.Limiter.Attack;

    [SerializedField(28, 3)] [ScalingFactor(100)] [SerializedType(typeof(int))]
    private double _hold = source.Dynamics.Limiter.Hold;

    [SerializedField(32, 4)] [ScalingFactor(100)] [SerializedType(typeof(int))]
    private double _release = source.Dynamics.Limiter.Release;
}
