using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CILP")]
public class FairlightMixerSourceLimiterCommand(Source source) : FairlightMixerSourceCommandBase(source)
{
    private double _release = source.Dynamics.Limiter.Release;
    private double _hold = source.Dynamics.Limiter.Hold;
    private double _attack = source.Dynamics.Limiter.Attack;
    private double _threshold = source.Dynamics.Limiter.Threshold;
    private bool _limiterEnabled = source.Dynamics.Limiter.Enabled;

    public double Release
    {
        get { return _release; }
        set
        {
            _release = value;
            Flag |= 1 << 4;
        }
    }

    public double Hold
    {
        get { return _hold; }
        set
        {
            _hold = value;
            Flag |= 1 << 3;
        }
    }

    public double Attack
    {
        get { return _attack; }
        set
        {
            _attack = value;
            Flag |= 1 << 2;
        }
    }

    public double Threshold
    {
        get { return _threshold; }
        set
        {
            _threshold = value;
            Flag |= 1 << 1;
        }
    }

    public bool LimiterEnabled
    {
        get { return _limiterEnabled; }
        set
        {
            _limiterEnabled = value;
            Flag |= 1 << 0;
        }
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var rawCommand = new byte[36];
        SerializeIds(rawCommand);
        rawCommand.WriteUInt8((byte)Flag, 0);
        rawCommand.WriteBoolean(LimiterEnabled, 16);
        rawCommand.WriteInt32BigEndian((int)(Threshold * 100), 20);
        rawCommand.WriteInt32BigEndian((int)(Attack * 100), 24);
        rawCommand.WriteInt32BigEndian((int)(Hold * 100), 28);
        rawCommand.WriteInt32BigEndian((int)(Release * 100), 32);
        return rawCommand;
    }
}
