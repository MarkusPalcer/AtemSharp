using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight.Master;

[Command("AMBP")]
public partial class FairlightMixerMasterEqualizerBandUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _bandIndex;

    [DeserializedField(1)]
    private bool _enabled;

    [DeserializedField(2)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.GetComponentsLegacy)}")]
    [SerializedType(typeof(byte))]
    private byte[] _supportedShapes = [];

    [DeserializedField(3)]
    private byte _shape;

    [DeserializedField(4)]
    [CustomScaling($"{nameof(AtemUtil)}.{nameof(AtemUtil.GetComponentsLegacy)}")]
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

    public void ApplyToState(AtemState state)
    {
        var equalizer = state.GetFairlight().Master.Equalizer;
        if (_bandIndex >= equalizer.Bands.Length)
        {
            throw new IndexOutOfRangeException($"Band Index {_bandIndex} does not exist on Master equalizer");
        }

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
