using AtemSharp.State.Video.DownstreamKeyer;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the fill source input for a downstream keyer
/// </summary>
[Command("CDsF")]
[BufferSize(4)]
public partial class DownstreamKeyFillSourceCommand(DownstreamKeyer dsk) : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    private readonly byte _downstreamKeyerId = dsk.Id;

    /// <summary>
    /// Fill source input number
    /// </summary>
    [SerializedField(2)]
    private ushort _input = dsk.Sources.FillSource;
}
