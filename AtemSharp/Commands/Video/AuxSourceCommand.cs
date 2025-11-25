using AtemSharp.State;

namespace AtemSharp.Commands.Video;

/// <summary>
/// Command to set the source for an auxiliary output
/// </summary>
[Command("CAuS")]
[BufferSize(4)]
public partial class AuxSourceCommand : SerializedCommand
{
    [SerializedField(1)] [NoProperty] private readonly byte _auxId;

    /// <summary>
    /// Source input number for the auxiliary output
    /// </summary>
    [SerializedField(2, 0)] private ushort _source;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    public AuxSourceCommand(AuxiliaryOutput auxiliaryOutput)
    {
        _auxId = auxiliaryOutput.Id;
        _source = auxiliaryOutput.Source;
    }
}
