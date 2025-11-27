using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the cut source input for a downstream keyer
/// </summary>
[Command("CDsC")]
[BufferSize(4)]
public partial class DownstreamKeyCutSourceCommand(DownstreamKeyer dsk) : SerializedCommand
{
    /// <summary>
    /// Downstream keyer index (0-based)
    /// </summary>
    [SerializedField(0)] [NoProperty] private readonly byte _downstreamKeyerId = dsk.Id;

    /// <summary>
    /// Cut source input number
    /// </summary>
    [SerializedField(2, 0)] private ushort _input = dsk.Sources.CutSource;
}
