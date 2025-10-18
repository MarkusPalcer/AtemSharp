using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CILP")]
public class FairlightMixerSourceLimiterCommand(Source source) : FairlightMixerSourceCommandBase(source)
{
    public LimiterParameters Parameters { get; } = new(source.Dynamics.Limiter);

    public override byte[] Serialize(ProtocolVersion version)
    {
        // For testability: If flag has been set from outside, use that instead of the internal one
        if (Flag != 0) Parameters.Flag = (byte)Flag;
        var rawCommand = new byte[36];
        SerializeIds(rawCommand);
        rawCommand.WriteUInt8(Parameters.Flag, 0);
        rawCommand.WriteBoolean(Parameters.LimiterEnabled, 16);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Threshold * 100), 20);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Attack * 100), 24);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Hold * 100), 28);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Release * 100), 32);
        return rawCommand;
    }


}
