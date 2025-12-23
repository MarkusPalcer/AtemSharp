using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("AMLP")]
internal partial class FairlightMixerMasterLimiterUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private bool _limiterEnabled;

    [DeserializedField(4)][SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _threshold;

    [DeserializedField(8)][SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _attack;

    [DeserializedField(12)][SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _hold;

    [DeserializedField(16)][SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _release;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var limiter = state.GetFairlight().Master.Dynamics.Limiter;
        limiter.Enabled = LimiterEnabled;
        limiter.Threshold = Threshold;
        limiter.Attack = Attack;
        limiter.Hold = Hold;
        limiter.Release = Release;
    }
}
