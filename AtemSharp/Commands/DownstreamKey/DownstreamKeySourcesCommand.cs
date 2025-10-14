using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("DskB")]
public class DownstreamKeySourcesCommand : IDeserializedCommand
{
    public static DownstreamKeySourcesCommand Deserialize(ReadOnlySpan<byte> buffer, ProtocolVersion version)
    {
        var downstreamKeyerId = buffer[0];
        var fillSource = buffer.ReadUInt16BigEndian(2);
        var cutSource = buffer.ReadUInt16BigEndian(4);

        return new DownstreamKeySourcesCommand
        {
            DownstreamKeyerId = downstreamKeyerId,
            FillSource = fillSource,
            CutSource = cutSource
        };
    }

    public static DownstreamKeySourcesCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        Span<byte> buffer = stackalloc byte[(int)stream.Length];
        stream.Read(buffer);
        return Deserialize(buffer, protocolVersion);
    }

    public byte DownstreamKeyerId { get; set; }
    public ushort FillSource { get; set; }
    public ushort CutSource { get; set; }

    public void ApplyToState(AtemState state)
    {
        state.Video.DownstreamKeyers[DownstreamKeyerId].Sources.FillSource = FillSource;
        state.Video.DownstreamKeyers[DownstreamKeyerId].Sources.CutSource = CutSource;
    }
}
