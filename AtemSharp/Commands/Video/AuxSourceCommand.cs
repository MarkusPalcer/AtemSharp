using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Video;

/// <summary>
/// Command to set the source for an auxiliary output
/// </summary>
[Command("CAuS")]
[BufferSize(4)]
public partial class AuxSourceCommand : SerializedCommand
{
    [SerializedField(1)]
    [NoProperty]
    internal readonly byte AuxBus;

    /// <summary>
    /// Source input number for the auxiliary output
    /// </summary>
    [SerializedField(2,0)]
    private ushort _source;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="auxBus">Auxiliary output index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if auxiliary output not available</exception>
    public AuxSourceCommand(byte auxBus, AtemState currentState)
    {
        AuxBus = auxBus;

        if (!currentState.Video.Auxiliaries.TryGetValue(auxBus, out var auxSource))
        {
            throw new IndexOutOfRangeException($"There is no auxiliary output with index {auxBus}.");
        }

        _source = auxSource;
    }
}
