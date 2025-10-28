using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the on-air state of a downstream keyer
/// </summary>
[Command("CDsL")]
[BufferSize(4)]
public partial class DownstreamKeyOnAirCommand : SerializedCommand
{

    [SerializedField(0)]
    [NoProperty]
    internal readonly byte DownstreamKeyerId;

    /// <summary>
    /// Whether the downstream keyer is on air
    /// </summary>
    [SerializedField(1)]
    private bool _onAir;


    public DownstreamKeyOnAirCommand(DownstreamKeyer dsk)
    {
        DownstreamKeyerId = dsk.Id;
        _onAir = dsk.OnAir;
    }
}
