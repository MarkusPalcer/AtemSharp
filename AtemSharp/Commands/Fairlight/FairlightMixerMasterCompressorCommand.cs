using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Fairlight;

[Command("CMCP")]
public class FairlightMixerMasterCompressorCommand : SerializedCommand
{
    public CompressorParameters Parameters { get; }

    public FairlightMixerMasterCompressorCommand(AtemState state)
    {
        Parameters = new CompressorParameters(state.GetFairlight().Master.Dynamics.Compressor);
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        if (Flag != 0) // Testability: Use flags from test if present
        {
            Parameters.Flag = (byte)Flag;
        }

        var buffer = new byte[24];
        buffer.WriteUInt8(Parameters.Flag, 0);
        buffer.WriteBoolean(Parameters.CompressorEnabled, 1);
        buffer.WriteInt32BigEndian((int)(Parameters.Threshold * 100), 4);
        buffer.WriteInt16BigEndian((short)(Parameters.Ratio * 100), 8);
        buffer.WriteInt32BigEndian((int)(Parameters.Attack * 100), 12);
        buffer.WriteInt32BigEndian((int)(Parameters.Hold * 100), 16);
        buffer.WriteInt32BigEndian((int)(Parameters.Release * 100), 20);

        return buffer;
    }
}
