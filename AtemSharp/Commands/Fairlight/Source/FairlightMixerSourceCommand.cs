using AtemSharp.Enums.Fairlight;
using AtemSharp.Helpers;
using AtemSharp.Lib;

namespace AtemSharp.Commands.Fairlight.Source;

[Command("CFSP")]
[BufferSize(48)]
public partial class FairlightMixerSourceCommand(State.Audio.Fairlight.Source source) : SerializedCommand
{
    [SerializedField(2)]
    [NoProperty]
    private readonly ushort _inputId = source.InputId;

    [SerializedField(8)]
    [NoProperty]
    private readonly long _sourceId = source.Id;

    [SerializedField(16,0)]
    private byte _framesDelay = source.FramesDelay;

    [SerializedField(20, 1)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _gain = source.Gain;

    [SerializedField(24, 2)]
    [SerializedType(typeof(short))]
    [ScalingFactor(100.0)]
    private double _stereoSimulation = source.StereoSimulation;

    [SerializedField(26,3)]
    private bool _equalizerEnabled = source.Equalizer.Enabled;

    [SerializedField(28,4)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _equalizerGain = source.Equalizer.Gain;

    [SerializedField(32,5)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _makeUpGain = source.Dynamics.MakeUpGain;

    [SerializedField(36,6)]
    [SerializedType(typeof(short))]
    [ScalingFactor(100.0)]
    private double _balance = source.Balance;

    [SerializedField(40,7)]
    [SerializedType(typeof(int))]
    [ScalingFactor(100.0)]
    private double _faderGain = source.FaderGain;

    [SerializedField(44,8)]
    private FairlightAudioMixOption _mixOption = source.MixOption;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
    }
}
