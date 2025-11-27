using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Source;

[Command("FASP")]
public partial class FairlightMixerSourceUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _inputId;

    [DeserializedField(8)] private long _sourceId;

    [DeserializedField(49)] private FairlightAudioMixOption _mixOption;

    [DeserializedField(48)] [SerializedType(typeof(FairlightAudioMixOption))] [CustomScaling("DeserializationExtensions.GetComponents")]
    private FairlightAudioMixOption[] _supportedMixOptions = [];

    [DeserializedField(44)] [SerializedType(typeof(int))] [ScalingFactor(100)]
    private double _faderGain;

    [DeserializedField(40)] [SerializedType(typeof(short))] [ScalingFactor(100)]
    private double _balance;

    [DeserializedField(36)] [SerializedType(typeof(int))] [ScalingFactor(100)]
    private double _makeUpGain;

    [DeserializedField(32)] [SerializedType(typeof(int))] [ScalingFactor(100)]
    private double _equalizerGain;

    [DeserializedField(29)] private bool _equalizerEnabled;

    [DeserializedField(28)] private byte _bandCount;

    [DeserializedField(26)] [SerializedType(typeof(short))] [ScalingFactor(100)]
    private double _stereoSimulation;

    [DeserializedField(24)] private bool _hasStereoSimulation;

    [DeserializedField(18)] private byte _framesDelay;

    [DeserializedField(17)] private byte _maxFramesDelay;

    [DeserializedField(16)] private FairlightAudioSourceType _sourceType;

    [DeserializedField(20)] [SerializedType(typeof(int))] [ScalingFactor(100)]
    private double _gain;

    /// <inheritdoc />
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

        source.Equalizer.Enabled = EqualizerEnabled;
        source.Equalizer.Gain = EqualizerGain;
        if (source.Equalizer.Bands.Length < BandCount)
        {
            source.Equalizer.Bands = AtemStateUtil.CreateArray<SourceEqualizerBand>(BandCount);
            foreach (var band in source.Equalizer.Bands)
            {
                band.InputId = InputId;
                band.SourceId = SourceId;
            }
        }

        source.Dynamics.MakeUpGain = MakeUpGain;
        source.Type = SourceType;
        source.MaxFramesDelay = MaxFramesDelay;
        source.FramesDelay = FramesDelay;
        source.Gain = Gain;
        source.HasStereoSimulation = HasStereoSimulation;
        source.StereoSimulation = StereoSimulation;
        source.Balance = Balance;
        source.FaderGain = FaderGain;
        source.SupportedMixOptions = SupportedMixOptions;
        source.MixOption = MixOption;
    }
}
