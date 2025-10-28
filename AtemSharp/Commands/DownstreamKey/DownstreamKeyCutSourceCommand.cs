using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to set the cut source input for a downstream keyer
/// </summary>
[Command("CDsC")]
[BufferSize(4)]
public partial class DownstreamKeyCutSourceCommand : SerializedCommand
{
    /// <summary>
    /// Downstream keyer index (0-based)
    /// </summary>
    [SerializedField(0)]
    [NoProperty]
    internal readonly byte DownstreamKeyerId;

    /// <summary>
    /// Cut source input number
    /// </summary>
    [SerializedField(2, 0)]
    private ushort _input;

    public DownstreamKeyCutSourceCommand(DownstreamKeyer dsk)
    {
        // Initialize from current state (direct field access = no flags set)
        DownstreamKeyerId = dsk.Id;
        _input = dsk.Sources.CutSource;
    }
}
