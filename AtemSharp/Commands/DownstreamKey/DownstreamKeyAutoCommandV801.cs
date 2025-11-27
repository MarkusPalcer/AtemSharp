using AtemSharp.State.Info;
using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to trigger an auto transition on a downstream keyer
/// </summary>
/// <remarks>
/// Used for protocol version 8.0.1 and higher
/// </remarks>
[Command("DDsA", ProtocolVersion.V8_0_1)]
[BufferSize(4)]
public partial class DownstreamKeyAutoCommandV801(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(1)] private byte _id = dsk.Id;
    [SerializedField(2, 0)] private bool _isTowardsOnAir = dsk.IsTowardsOnAir;
}
