using AtemSharp.Enums;
using AtemSharp.Enums.Fairlight;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CFSP")]
public class FairlightMixerSourceCommand : FairlightMixerSourceCommandBase
{
    private byte _framesDelay;
    private double _gain;
    private double _stereoSimulation;
    private bool _equalizerEnabled;
    private double _equalizerGain;
    private double _makeUpGain;

    public byte FramesDelay
    {
        get => _framesDelay;
        set
        {
            _framesDelay = value;
            Flag |= 1 << 0;
        }
    }

    public double Gain
    {
        get => _gain;
        set
        {
            _gain = value;
            Flag |= 1 << 1;
        }
    }

    public double StereoSimulation
    {
        get => _stereoSimulation;
        set
        {
            _stereoSimulation = value;
            Flag |= 1 << 2;
        }
    }

    public bool EqualizerEnabled
    {
        get => _equalizerEnabled;
        set
        {
            _equalizerEnabled = value;
            Flag |= 1 << 3;
        }
    }

    public double EqualizerGain
    {
        get => _equalizerGain;
        set
        {
            _equalizerGain = value;
            Flag |= 1 << 4;
        }
    }

    public double MakeUpGain
    {
        get => _makeUpGain;
        set
        {
            _makeUpGain = value;
            Flag |= 1 << 5;
        }
    }

    public double Balance
    {
        get => _balance;
        set
        {
            _balance = value;
            Flag |= 1 << 6;
        }
    }
    private double _balance;

    public double FaderGain
    {
        get => _faderGain;
        set
        {
            _faderGain = value;
            Flag |= 1 << 7;
        }
    }
    private double _faderGain;
    private FairlightAudioMixOption _mixOption;

    public FairlightAudioMixOption MixOption
    {
        get => _mixOption;
        set
        {
            _mixOption = value;
            Flag |= 1 << 8;
        }
    }

    public FairlightMixerSourceCommand(Source source) : base(source)
    {
        _framesDelay = source.FramesDelay;
        _gain = source.Gain;
        _stereoSimulation = source.StereoSimulation;
        _equalizerEnabled = source.Equalizer.Enabled;
        _equalizerGain = source.Equalizer.Gain;
        _makeUpGain = source.Dynamics.MakeUpGain;
        _balance = source.Balance;
        _faderGain = source.FaderGain;
        _mixOption = source.MixOption;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[48];

        SerializeIds(buffer);
        buffer.WriteUInt16BigEndian(Flag, 0);

        buffer.WriteUInt8(_framesDelay, 16);
        buffer.WriteInt32BigEndian((int)(_gain * 100), 20);
        buffer.WriteInt16BigEndian((short)(_stereoSimulation * 100), 24);

        buffer.WriteBoolean(_equalizerEnabled, 26);
        buffer.WriteInt32BigEndian((int)(_equalizerGain * 100), 28);
        buffer.WriteInt32BigEndian((int)(_makeUpGain * 100), 32);
        buffer.WriteInt16BigEndian((short)(_balance * 100), 36);
        buffer.WriteInt32BigEndian((int)(_faderGain * 100), 40);
        buffer.WriteUInt8((byte)_mixOption, 44);
        return buffer;
    }
}
