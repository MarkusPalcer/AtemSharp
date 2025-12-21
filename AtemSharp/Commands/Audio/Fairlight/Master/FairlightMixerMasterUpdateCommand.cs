using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("FAMP")]
public partial class FairlightMixerMasterUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _bandCount;

    [DeserializedField(1)] private bool _equalizerEnabled;

    [DeserializedField(4)] [SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _equalizerGain;

    [DeserializedField(8)] [SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _makeUpGain;

    [DeserializedField(12)] [SerializedType(typeof(int))] [ScalingFactor(100.0)]
    private double _faderGain;

    [DeserializedField(16)] private bool _followFadeToBlack;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var fairlight = state.GetFairlight();
        fairlight.Master.FaderGain = FaderGain;
        fairlight.Master.Dynamics.MakeUpGain = MakeUpGain;
        fairlight.Master.Equalizer.Enabled = EqualizerEnabled;
        fairlight.Master.Equalizer.Gain = EqualizerGain;
        fairlight.Master.FollowFadeToBlack = FollowFadeToBlack;

        fairlight.Master.Equalizer.Bands.Populate(BandCount);
    }
}
