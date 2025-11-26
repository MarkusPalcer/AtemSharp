using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Source;

[Command("FMLv")]
public partial class FairlightMixerSourceLevelsUpdateCommand : IDeserializedCommand
{
    [DeserializedField(8)]
    private ushort _inputId;

    [DeserializedField(0)]
    private long _sourceId;

    [DeserializedField(10)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputLeftLevel;
    [DeserializedField(12)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputRightLevel;
    [DeserializedField(14)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputLeftPeak;
    [DeserializedField(16)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _inputRightPeak;

    [DeserializedField(18)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _expanderGainReduction;
    [DeserializedField(20)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _compressorGainReduction;
    [DeserializedField(22)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _limiterGainReduction;

    [DeserializedField(24)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputLeftLevel;
    [DeserializedField(26)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputRightLevel;
    [DeserializedField(28)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputLeftPeak;
    [DeserializedField(30)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _outputRightPeak;

    [DeserializedField(32)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _leftLevel;
    [DeserializedField(34)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _rightLevel;
    [DeserializedField(36)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _leftPeak;
    [DeserializedField(38)] [ScalingFactor(100)] [SerializedType(typeof(short))] private double _rightPeak;

    public void ApplyToState(AtemState state)
    {
        // Not applied to state
    }
}
