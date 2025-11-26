using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the on-air state of a downstream keyer
/// </summary>
[Command("CDsL")]
[BufferSize(4)]
public partial class DownstreamKeyOnAirCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _downstreamKeyerId = dsk.Id;

    /// <summary>
    /// Whether the downstream keyer is on air
    /// </summary>
    [SerializedField(1)] private bool _onAir = dsk.OnAir;
}
