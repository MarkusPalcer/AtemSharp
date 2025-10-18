using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CMLP")]
public class FairlightMixerMasterLimiterCommand(MasterProperties master) : SerializedCommand
{
    public LimiterParameters Parameters { get; } = new(master.Dynamics.Limiter);

    public override byte[] Serialize(ProtocolVersion version)
    {
        // For testability: If flag has been set from outside, use that instead of the internal one
        if (Flag != 0) Parameters.Flag = (byte)Flag;
        var rawCommand = new byte[20];
        rawCommand.WriteUInt8(Parameters.Flag, 0);
        rawCommand.WriteBoolean(Parameters.LimiterEnabled, 1);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Threshold * 100), 4);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Attack * 100), 8);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Hold * 100), 12);
        rawCommand.WriteInt32BigEndian((int)(Parameters.Release * 100), 16);
        return rawCommand;
    }
}
