using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Used to set the on-air state of a downstream keyer
/// </summary>
[Command("CDsL")]
[BufferSize(4)]
public partial class DownstreamKeyOnAirCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _downstreamKeyerId = dsk.Id;
    [SerializedField(1)] private bool _onAir = dsk.OnAir;
}
