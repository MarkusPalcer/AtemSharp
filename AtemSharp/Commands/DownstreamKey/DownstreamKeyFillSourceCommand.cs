using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the fill source input for a downstream keyer
/// </summary>
[Command("CDsF")]
[BufferSize(4)]
public partial class DownstreamKeyFillSourceCommand : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    internal readonly byte DownstreamKeyerId;

    /// <summary>
    /// Fill source input number
    /// </summary>
    [SerializedField(2)]
    private ushort _input;

    public DownstreamKeyFillSourceCommand(DownstreamKeyer dsk)
    {
        _input = dsk.Sources.FillSource;
        DownstreamKeyerId = dsk.Id;
    }
}
