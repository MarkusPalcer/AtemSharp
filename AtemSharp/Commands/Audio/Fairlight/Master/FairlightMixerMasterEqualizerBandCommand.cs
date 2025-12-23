using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

/// <summary>
/// Used to set the properties of one equalizer band on the master channel of the fairlight mixer
/// </summary>
[Command("CMBP")]
[BufferSize(20)]
public partial class FairlightMixerMasterEqualizerBandCommand(MasterEqualizerBand band) : SerializedCommand
{
    [SerializedField(1)]
    [NoProperty]
    private readonly byte _bandIndex = band.Id;

    [SerializedField(2, 0)]
    private bool _enabled = band.Enabled;

    [SerializedField(3, 1)]
    private Shape _shape = band.Shape;

    [SerializedField(4, 2)]
    private byte _frequencyRange = band.FrequencyRange;

    [SerializedField(8,3)]
    [SerializedType(typeof(uint))]
    private uint _frequency = band.Frequency;

    [SerializedField(12, 4)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(int))]
    private double _gain = band.Gain;

    [SerializedField(16, 5)]
    [ScalingFactor(100.0)]
    [SerializedType(typeof(short))]
    private double _qFactor = band.QFactor;
}
