using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

[Command("CDsT")]
[BufferSize(4)]
public partial class DownstreamKeyTieCommand(DownstreamKeyer keyer) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _downStreamKeyerId = keyer.Id;

    [SerializedField(1)] private bool _tie = keyer.Properties.Tie;
}
