using AtemSharp.Lib;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Fairlight;

[Command("RFLP")]
public class FairlightMixerResetPeakLevelsCommand : SerializedCommand
{
    public bool All { get; set; }
    public bool Master { get; set; }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        byte val = 0;

        if (All)
        {
            val |= 1 << 0;

            // some magic number that is needed for this to work
            buffer.WriteUInt8(0x01, 1);
        }

        if (Master)
        {
            val |= 1 << 1;

            // some magic number that is needed for this to work
            buffer.WriteUInt8(0x04, 3);
        }

        buffer.WriteUInt8(val, 0);

        return buffer;
    }
}
