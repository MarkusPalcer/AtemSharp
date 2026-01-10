using AtemSharp.State.Info;

namespace AtemSharp.Commands.Audio.Fairlight;

/// <summary>
/// Used to reset the peaks of all channels and/or the master channel of the fairlight mixer
/// </summary>
[Command("RFLP")]
public class FairlightMixerResetPeakLevelsCommand : SerializedCommand
{
    public bool All { get; set; }
    public bool Master { get; set; }

    /// <inheritdoc />
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

    internal override bool TryMergeTo(SerializedCommand other)
    {
        if (other is not FairlightMixerResetPeakLevelsCommand target)
        {
            return false;
        }

        target.All |= All;
        target.Master |= Master;

        return true;
    }
}
