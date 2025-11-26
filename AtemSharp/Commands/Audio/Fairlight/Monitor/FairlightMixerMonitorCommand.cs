using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Monitor;

[Command("CFMH")]
[BufferSize(36)]
public partial class FairlightMixerMonitorCommand(FairlightAudioState state) : SerializedCommand
{
    [SerializedField(4, 0)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _gain = state.Monitor.Gain;
    [SerializedField(8, 1)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _inputMasterGain = state.Monitor.InputMasterGain;
    [SerializedField(12, 2)] private bool _inputMasterMuted = state.Monitor.InputMasterMuted;
    [SerializedField(16, 3)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _inputTalkbackGain = state.Monitor.InputTalkbackGain;
    [SerializedField(20, 4)] private bool _inputTalkbackMuted = state.Monitor.InputTalkbackMuted;
    [SerializedField(32, 7)] [SerializedType(typeof(int))] [ScalingFactor(100)] private double _inputSidetoneGain = state.Monitor.InputSidetoneGain;
}
