using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CICP")]
public class FairlightMixerSourceCompressorCommand : FairlightMixerSourceCommandBase
{
    public CompressorParameters Parameters { get; }

    public FairlightMixerSourceCompressorCommand(Source source) : base(source)
    {
        Parameters = new CompressorParameters(source.Dynamics.Compressor);
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        if (Flag != 0) // Testability: Use flags from test if present
        {
            Parameters.Flag = (byte)Flag;
        }

        byte[] buffer = new byte[40];
        buffer.WriteUInt8(Parameters.Flag, 0);
        SerializeIds(buffer);
        buffer.WriteBoolean(Parameters.CompressorEnabled, 16);
        buffer.WriteInt32BigEndian((int)(Parameters.Threshold * 100), 20);
        buffer.WriteInt16BigEndian((short)(Parameters.Ratio * 100), 24);
        buffer.WriteInt32BigEndian((int)(Parameters.Attack * 100), 28);
        buffer.WriteInt32BigEndian((int)(Parameters.Hold * 100), 32);
        buffer.WriteInt32BigEndian((int)(Parameters.Release * 100), 36);

        return buffer;
    }
}
