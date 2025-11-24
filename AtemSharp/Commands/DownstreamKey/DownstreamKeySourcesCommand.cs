using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("DskB")]
public partial class DownstreamKeySourcesCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private byte _downstreamKeyerId;

    [DeserializedField(2)]
    private ushort _fillSource;

    [DeserializedField(4)]
    private ushort _cutSource;

    public void ApplyToState(AtemState state)
    {
        state.Video.DownstreamKeyers[DownstreamKeyerId].Sources.FillSource = FillSource;
        state.Video.DownstreamKeyers[DownstreamKeyerId].Sources.CutSource = CutSource;
    }
}
