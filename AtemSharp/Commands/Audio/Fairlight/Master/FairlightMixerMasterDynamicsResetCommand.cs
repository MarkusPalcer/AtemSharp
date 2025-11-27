using AtemSharp.State.Info;

namespace AtemSharp.Commands.Audio.Fairlight.Master;

[Command("RMOD")]
public class FairlightMixerMasterDynamicsResetCommand : SerializedCommand
{
    public bool ResetDynamics { get; set; }
    public bool ResetExpander { get; set; }
    public bool ResetCompressor { get; set; }
    public bool ResetLimiter { get; set; }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];
        byte val = 0;
        if (ResetDynamics) val |= 1 << 0;
        if (ResetExpander) val |= 1 << 1;
        if (ResetCompressor) val |= 1 << 2;
        if (ResetLimiter) val |= 1 << 3;

        buffer.WriteUInt8(val, 1);
        return buffer;
    }
}
