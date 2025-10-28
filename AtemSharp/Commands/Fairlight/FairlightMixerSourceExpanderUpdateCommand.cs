using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("AIXP")]
public partial class FairlightMixerSourceExpanderUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private ushort _inputId;

    [DeserializedField(8)]
    private long _sourceId;

    [DeserializedField(36)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _release;

    [DeserializedField(32)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _hold;

    [DeserializedField(28)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _attack;

    [DeserializedField(26)]
    [SerializedType(typeof(short))]
    [ScalingFactor(100)]
    private double _ratio;

    [DeserializedField(24)]
    [SerializedType(typeof(short))]
    [ScalingFactor(100)]
    private double _range;

    [DeserializedField(20)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100)]
    private double _threshold;

    [DeserializedField(17)]
    private bool _gateEnabled;

    [DeserializedField(16)]
    private bool _expanderEnabled;

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

        source.Dynamics.Expander.Enabled = ExpanderEnabled;
        source.Dynamics.Expander.GateEnabled = GateEnabled;
        source.Dynamics.Expander.Threshold = Threshold;
        source.Dynamics.Expander.Range = Range;
        source.Dynamics.Expander.Ratio = Ratio;
        source.Dynamics.Expander.Attack = Attack;
        source.Dynamics.Expander.Hold = Hold;
        source.Dynamics.Expander.Release = Release;
    }
}
