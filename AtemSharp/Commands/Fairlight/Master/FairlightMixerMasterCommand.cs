using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight.Master;

[Command("CFMP")]
[BufferSize(20)]
public partial class FairlightMixerMasterCommand(MasterProperties master) : SerializedCommand
{
    [SerializedField(1,0)]
    private bool _equalizerEnabled = master.Equalizer.Enabled;

    [SerializedField(4,1)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _equalizerGain = master.Equalizer.Gain;

    [SerializedField(8,2)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _makeUpGain = master.Dynamics.MakeUpGain;

    [SerializedField(12,3)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _faderGain = master.FaderGain;

    [SerializedField(16,4)]
    private bool _followFadeToBlack = master.FollowFadeToBlack;
}
