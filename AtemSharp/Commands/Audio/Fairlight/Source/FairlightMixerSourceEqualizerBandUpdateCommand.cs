using AtemSharp.State;

namespace AtemSharp.Commands.Audio.Fairlight.Source;

[Command("AEBP")]
public partial class FairlightMixerSourceEqualizerBandUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _inputId;

    [DeserializedField(8)] private long _sourceId;

    [DeserializedField(16)] private byte _bandIndex;

    [DeserializedField(17)] private bool _enabled;

    [DeserializedField(18)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponentsLegacy)}")]
    [SerializedType(typeof(byte))]
    private byte[] _supportedShapes = [];

    [DeserializedField(19)] private byte _shape;

    [DeserializedField(20)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponentsLegacy)}")]
    [SerializedType(typeof(byte))]
    private byte[] _supportedFrequencyRanges = [];

    [DeserializedField(21)] private byte _frequencyRange;

    [DeserializedField(24)] private uint _frequency;

    [DeserializedField(28)] [SerializedType(typeof(int))] [ScalingFactor(100)]
    private double _gain;

    [DeserializedField(32)] [SerializedType(typeof(short))] [ScalingFactor(100)]
    private double _qFactor;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var audio = state.GetFairlight();

        var source = audio.Inputs.GetValueOrDefault(InputId)?.Sources.GetValueOrDefault(_sourceId);
        if (source is null)
        {
            throw new IndexOutOfRangeException($"Source ID {SourceId} on Input ID {InputId} does not exist");
        }

        if (BandIndex >= source.Equalizer.Bands.Length)
        {
            throw new IndexOutOfRangeException($"Band Index {BandIndex} does not exist on Source ID {SourceId} on Input ID {InputId}");
        }

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
