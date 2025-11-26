using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("FDLv")]
public partial class FairlightMixerMasterLevelsUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputLeftLevel;
    [DeserializedField(2)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputRightLevel;
    [DeserializedField(4)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputLeftPeak;
    [DeserializedField(6)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputRightPeak;

    [DeserializedField(8)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _compressorGainReduction;
    [DeserializedField(10)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _limiterGainReduction;

    [DeserializedField(12)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputLeftLevel;
    [DeserializedField(14)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputRightLevel;
    [DeserializedField(16)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputLeftPeak;
    [DeserializedField(18)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputRightPeak;

    [DeserializedField(20)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _leftLevel;
    [DeserializedField(22)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _rightLevel;
    [DeserializedField(24)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _leftPeak;
    [DeserializedField(26)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _rightPeak;

    public void ApplyToState(AtemState state)
    {
        // Not represented in state
    }
}
