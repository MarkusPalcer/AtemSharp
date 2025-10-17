using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CIXP")]
public class FairlightMixerSourceExpanderCommand : SerializedCommand
{
    private bool _expanderEnabled;
    private bool _gateEnabled;
    private double _threshold;
    private double _range;
    private double _ratio;
    private double _attack;
    private double _hold;
    private double _release;
    private ushort _inputId;
    private long _sourceId;

    public FairlightMixerSourceExpanderCommand(Source source)
    {
        _expanderEnabled = source.Dynamics.Expander.Enabled;
        _gateEnabled = source.Dynamics.Expander.GateEnabled;
        _threshold = source.Dynamics.Expander.Threshold;
        _range = source.Dynamics.Expander.Range;
        _ratio = source.Dynamics.Expander.Ratio;
        _attack = source.Dynamics.Expander.Attack;
        _hold = source.Dynamics.Expander.Hold;
        _release = source.Dynamics.Expander.Release;
        _inputId = source.InputId;
        _sourceId = source.Id;
    }

    public bool ExpanderEnabled
    {
        get { return _expanderEnabled; }
        set
        {
            _expanderEnabled = value;
            Flag |= 1 << 0;
        }
    }

    public bool GateEnabled
    {
        get { return _gateEnabled; }
        set
        {
            _gateEnabled = value;
            Flag |= 1 << 1;
        }
    }

    public double Threshold
    {
        get { return _threshold; }
        set
        {
            _threshold = value;
            Flag |= 1 << 2;
        }
    }

    public double Range
    {
        get { return _range; }
        set
        {
            _range = value;
            Flag |= 1 << 3;
        }
    }

    public double Ratio
    {
        get { return _ratio; }
        set
        {
            _ratio = value;
            Flag |= 1 << 4;
        }
    }

    public double Attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
            Flag |= 1 << 5;
        }
    }

    public double Hold
    {
        get { return _hold; }
        set
        {
            _hold = value;
            Flag |= 1 << 6;
        }
    }

    public double Release
    {
        get { return _release; }
        set
        {
            _release = value;
            Flag |= 1 << 7;
        }
    }


    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[40];
        buffer.WriteUInt8((byte)Flag, 0);
        buffer.WriteUInt16BigEndian(_inputId, 2);
        buffer.WriteInt64BigEndian(_sourceId, 8);

        buffer.WriteBoolean(_expanderEnabled, 16);
        buffer.WriteBoolean(_gateEnabled, 17);
        buffer.WriteInt32BigEndian((int)(_threshold * 100), 20);
        buffer.WriteInt16BigEndian((short)(_range * 100), 24);
        buffer.WriteInt16BigEndian((short)(_ratio * 100), 26);
        buffer.WriteInt32BigEndian((int)(_attack * 100), 28);
        buffer.WriteInt32BigEndian((int)(_hold * 100), 32);
        buffer.WriteInt32BigEndian((int)(_release * 100), 36);

        return buffer;
    }


}
