using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

[Command("CDsR")]
[BufferSize(4)]
public partial class DownstreamKeyRateCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    private readonly byte _downstreamKeyId = dsk.Id;

    [SerializedField(1)]
    private byte _rate = dsk.Properties.Rate;
}
