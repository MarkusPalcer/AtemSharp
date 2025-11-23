using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight.Monitor;

[Command("FMHP")]
public partial class FairlightMixerMonitorUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _gain;
    [DeserializedField(4)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _inputMasterGain;
    [DeserializedField(8)] private bool _inputMasterMuted;
    [DeserializedField(12)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _inputTalkbackGain;
    [DeserializedField(16)] private bool _inputTalkbackMuted;
    [DeserializedField(28)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _inputSidetoneGain;

    public void ApplyToState(AtemState state)
    {
        state.GetFairlight().Monitor.Gain = _gain;
        state.GetFairlight().Monitor.InputMasterGain = _inputMasterGain;
        state.GetFairlight().Monitor.InputMasterMuted = _inputMasterMuted;
        state.GetFairlight().Monitor.InputTalkbackGain = _inputTalkbackGain;
        state.GetFairlight().Monitor.InputTalkbackMuted = _inputTalkbackMuted;
        state.GetFairlight().Monitor.InputSidetoneGain = _inputSidetoneGain;
    }
}
