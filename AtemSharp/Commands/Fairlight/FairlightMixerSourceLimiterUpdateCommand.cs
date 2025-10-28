using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("AILP")]
public partial class FairlightMixerSourceLimiterUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private ushort _inputId;

    [DeserializedField(8)]
    private long _sourceId;

    [DeserializedField(16)]
    private bool _limiterEnabled;

    [DeserializedField(20)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _threshold;

    [DeserializedField(24)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _attack;

    [DeserializedField(28)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _hold;

    [DeserializedField(32)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _release;

    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();

        if (!audio.Inputs.TryGetValue(InputId, out var input))
        {
            throw new IndexOutOfRangeException($"Input ID {InputId} does not exist");
        }

        var source = input.Sources.GetOrCreate(SourceId);
        source.Id = SourceId;
        source.InputId = InputId;

        var limiter = source.Dynamics.Limiter;
        limiter.Enabled = LimiterEnabled;
        limiter.Threshold = Threshold;
        limiter.Attack = Attack;
        limiter.Hold = Hold;
        limiter.Release = Release;
    }
}
