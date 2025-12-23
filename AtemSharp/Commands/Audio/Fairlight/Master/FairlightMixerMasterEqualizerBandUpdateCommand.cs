using AtemSharp.State;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("AMBP")]
internal partial class FairlightMixerMasterEqualizerBandUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _bandIndex;

    [DeserializedField(1)]
    private bool _enabled;

    [DeserializedField(2)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponents)}")]
    [SerializedType(typeof(Shape))]
    private Shape[] _supportedShapes = [];

    [DeserializedField(3)]
    private Shape _shape;

    [DeserializedField(4)]
    [CustomScaling($"{nameof(DeserializationExtensions)}.{nameof(DeserializationExtensions.GetComponentsLegacy)}")]
    [SerializedType(typeof(byte))]
    private byte[] _supportedFrequencyRanges = [];

    [DeserializedField(5)]
    private byte _frequencyRange;

    [DeserializedField(8)]
    private uint _frequency;

    [DeserializedField(12)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _gain;

    [DeserializedField(16)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(short))]
    private double _qFactor;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var equalizer = state.GetFairlight().Master.Equalizer;
        var band =  equalizer.Bands[_bandIndex];

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
