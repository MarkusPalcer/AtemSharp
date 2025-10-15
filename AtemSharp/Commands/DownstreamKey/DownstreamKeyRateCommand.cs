using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("CDsR")]
public class DownstreamKeyRateCommand : SerializedCommand
{
    public byte DownstreamKeyId { get; }
    public byte Rate { get; set; }

    public DownstreamKeyRateCommand(AtemState state, byte downstreamKeyId)
    {
        DownstreamKeyId = downstreamKeyId;

        if (DownstreamKeyId > state.Video.DownstreamKeyers.Length)
        {
            throw new IndexOutOfRangeException("Downstream keyer ID is out of range");
        }

        var properties = state.Video.DownstreamKeyers[DownstreamKeyId].Properties;
        if (properties is null) throw new InvalidOperationException("Downstream keyer properties are not available");

        Rate = properties.Rate;
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];

        buffer.WriteUInt8(DownstreamKeyId, 0);
        buffer.WriteUInt8(Rate, 1);

        return buffer;
    }
}
