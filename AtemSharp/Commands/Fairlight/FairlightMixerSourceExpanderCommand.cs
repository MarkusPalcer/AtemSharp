using AtemSharp.Helpers;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CIXP")]
[BufferSize(40)]
public partial class FairlightMixerSourceExpanderCommand(Source source) : SerializedCommand
{

    [SerializedField(2)]
    [NoProperty]
    private readonly ushort _inputId = source.InputId;

    [SerializedField(8)] [NoProperty] private readonly long _sourceId = source.Id;

    [SerializedField(16,0)]
    private bool _expanderEnabled = source.Dynamics.Expander.Enabled;

    [SerializedField(17,1)]
    private bool _gateEnabled = source.Dynamics.Expander.GateEnabled;

    [SerializedField(20,2)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _threshold = source.Dynamics.Expander.Threshold;

    [SerializedField(24, 3)]
    [ScalingFactor(100)]
    [SerializedType(typeof(short))]
    private double _range = source.Dynamics.Expander.Range;

    [SerializedField(26, 4)]
    [ScalingFactor(100)]
    [SerializedType(typeof(short))]
    private double _ratio = source.Dynamics.Expander.Ratio;

    [SerializedField(28, 5)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _attack = source.Dynamics.Expander.Attack;

    [SerializedField(32, 6)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _hold = source.Dynamics.Expander.Hold;

    [SerializedField(36, 7)]
    [ScalingFactor(100)]
    [SerializedType(typeof(int))]
    private double _release = source.Dynamics.Expander.Release;
}
