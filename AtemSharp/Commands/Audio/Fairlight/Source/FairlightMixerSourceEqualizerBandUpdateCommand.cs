using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Source;

[Command("AEBP")]
internal partial class FairlightMixerSourceEqualizerBandUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _inputId;

    [DeserializedField(8)] private long _sourceId;

    [DeserializedField(16)] private byte _bandIndex;

    [DeserializedField(17)] private bool _enabled;

    [DeserializedField(18)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    [SerializedType(typeof(Shape))]
    private Shape[] _supportedShapes = [];

    [DeserializedField(19)] private Shape _shape;

    [DeserializedField(20)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    [SerializedType(typeof(FrequencyRange))]
    private FrequencyRange[] _supportedFrequencyRanges = [];

    [DeserializedField(21)] private FrequencyRange _frequencyRange;

    [DeserializedField(24)] private uint _frequency;

    [DeserializedField(28)] [SerializedType(typeof(int))] [ScalingFactor(100)]
    private double _gain;

    [DeserializedField(32)] [SerializedType(typeof(short))] [ScalingFactor(100)]
    private double _qFactor;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();
        var input = audio.Inputs[InputId];
        var source = input.Sources[_sourceId];
        var band = source.Equalizer.Bands[BandIndex];

        band.Enabled = Enabled;
        band.SupportedShapes = SupportedShapes;
        band.Shape = Shape;
        band.SupportedFrequencyRanges = SupportedFrequencyRanges;
        band.FrequencyRange = FrequencyRange;
        band.Frequency = Frequency;
        band.Gain = Gain;
        band.QFactor = QFactor;
    }
}
