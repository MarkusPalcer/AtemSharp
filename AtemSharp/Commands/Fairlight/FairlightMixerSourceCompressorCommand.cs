using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State.Audio.Fairlight;

namespace AtemSharp.Commands.Fairlight;

[Command("CICP")]
public class FairlightMixerSourceCompressorCommand : FairlightMixerSourceCommandBase
{
    public double Release { get; set; }

    public double Hold { get; set; }

    public double Attack { get; set; }

    public double Ratio { get; set; }

    public double Threshold { get; set; }

    public bool CompressorEnabled { get; set; }


    public FairlightMixerSourceCompressorCommand(Source source) : base(source)
    {
        CompressorEnabled = source.Dynamics.Compressor.Enabled;
        Threshold = source.Dynamics.Compressor.Threshold;
        Ratio = source.Dynamics.Compressor.Ratio;
        Attack = source.Dynamics.Compressor.Attack;
        Hold = source.Dynamics.Compressor.Hold;
        Release = source.Dynamics.Compressor.Release;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        byte[] buffer = new byte[40];
        buffer.WriteUInt8((byte)Flag, 0);
        SerializeIds(buffer);
        buffer.WriteBoolean(CompressorEnabled, 16);
        buffer.WriteInt32BigEndian((int)(Threshold * 100), 20);
        buffer.WriteInt16BigEndian((short)(Ratio * 100), 24);
        buffer.WriteInt32BigEndian((int)(Attack * 100), 28);
        buffer.WriteInt32BigEndian((int)(Hold * 100), 32);
        buffer.WriteInt32BigEndian((int)(Release * 100), 36);

        return buffer;
    }
}
