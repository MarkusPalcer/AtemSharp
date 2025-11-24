using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight.Master;

[Command("MOCP")]
public partial class FairlightMixerMasterCompressorUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private bool _enabled;

    [DeserializedField(4)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _threshold;

    [DeserializedField(8)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(short))]
    private double _ratio;

    [DeserializedField(12)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _attack;

    [DeserializedField(16)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _hold;

    [DeserializedField(20)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _release;

    public void ApplyToState(AtemState state)
    {
        var compressor = state.GetFairlight().Master.Dynamics.Compressor;

        compressor.Enabled = _enabled;
        compressor.Threshold = _threshold;
        compressor.Ratio = _ratio;
        compressor.Attack = _attack;
        compressor.Hold = _hold;
    }
}
