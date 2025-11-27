using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to trigger an auto transition on a downstream keyer
/// </summary>
/// <remarks>
/// Used for protocol versions up to (and including) 8.0.0
/// </remarks>
[Command("DDsA")]
[BufferSize(4)]
public partial class DownstreamKeyAutoCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(0)] private byte _id = dsk.Id;
}
