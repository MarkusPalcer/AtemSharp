using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("DskB")]
public class DownstreamKeySourcesCommand : IDeserializedCommand
{
    public static DownstreamKeySourcesCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new DownstreamKeySourcesCommand
        {
            DownstreamKeyerId = rawCommand.ReadUInt8(0),
            FillSource = rawCommand.ReadUInt16BigEndian(2),
            CutSource = rawCommand.ReadUInt16BigEndian(4)
        };
    }

    public byte DownstreamKeyerId { get; init; }
    public ushort FillSource { get; init; }
    public ushort CutSource { get; init; }

    public void ApplyToState(AtemState state)
    {
        state.Video.DownstreamKeyers[DownstreamKeyerId].Sources.FillSource = FillSource;
        state.Video.DownstreamKeyers[DownstreamKeyerId].Sources.CutSource = CutSource;
    }
}
