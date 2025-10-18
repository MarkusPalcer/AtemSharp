using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CFMP")]
public class FairlightMixerMasterCommand :SerializedCommand
{
    private bool _followFadeToBlack;
    private double _faderGain;
    private double _makeUpGain;
    private double _equalizerGain;
    private bool _equalizerEnabled;

    public bool FollowFadeToBlack
    {
        get { return _followFadeToBlack; }
        set
        {
            _followFadeToBlack = value;
            Flag |= 1 << 4;
        }
    }

    public double FaderGain
    {
        get { return _faderGain; }
        set
        {
            _faderGain = value;
            Flag |= 1 << 3;
        }
    }

    public double MakeUpGain
    {
        get { return _makeUpGain; }
        set
        {
            _makeUpGain = value;
            Flag |= 1 << 2;
        }
    }

    public double EqualizerGain
    {
        get { return _equalizerGain; }
        set
        {
            _equalizerGain = value;
            Flag |= 1 << 1;
        }
    }

    public bool EqualizerEnabled
    {
        get { return _equalizerEnabled; }
        set
        {
            _equalizerEnabled = value;
            Flag |= 1 << 0;
        }
    }

    public FairlightMixerMasterCommand(MasterProperties master)
    {
        _equalizerEnabled = master.Equalizer.Enabled;
        _equalizerGain = master.Equalizer.Gain;
        _makeUpGain = master.Dynamics.MakeUpGain;
        _faderGain = master.FaderGain;
        _followFadeToBlack = master.FollowFadeToBlack;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[20];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteBoolean(EqualizerEnabled, 1);
        buffer.WriteInt32BigEndian((int)(EqualizerGain * 100.0), 4);
        buffer.WriteInt32BigEndian((int)(MakeUpGain * 100.0), 8);
        buffer.WriteInt32BigEndian((int)(FaderGain * 100.0), 12);
        buffer.WriteBoolean(FollowFadeToBlack, 16);
        return buffer;
    }
}
