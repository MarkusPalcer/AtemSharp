using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight.Source;

[Command("CEBP")]
[BufferSize(32)]
public partial class FairlightMixerSourceEqualizerBandCommand(SourceEqualizerBand band) : SerializedCommand
{

    [SerializedField(2)]
    [NoProperty]
    private readonly ushort _inputId = band.InputId;

    [SerializedField(8)]
    [NoProperty]
    private readonly long _sourceId = band.SourceId;

    [SerializedField(16)]
    [NoProperty]
    private readonly byte _bandIndex = band.Id;

    [SerializedField(17, 0)]
    private bool _enabled = band.Enabled;

    [SerializedField(18, 1)]
    private byte _shape = band.Shape;

    [SerializedField(19, 2)]
    private byte _frequencyRange = band.FrequencyRange;

    [SerializedField(20,3)]
    private uint _frequency = band.Frequency;

    [SerializedField(24, 4)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _gain = band.Gain;

    [SerializedField(28, 5)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(short))]
    private double _qFactor = band.QFactor;
    }
