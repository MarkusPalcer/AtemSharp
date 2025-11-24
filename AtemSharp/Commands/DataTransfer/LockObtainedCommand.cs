using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer lock has been obtained
/// </summary>
[Command("LKOB")]
public partial class LockObtainedCommand : IDeserializedCommand
{
    /// <summary>
    /// Index of the lock that was obtained
    /// </summary>
    [DeserializedField(0)]
    private ushort _index;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a lock was obtained
        // The TypeScript implementation also returns an empty array
    }
}
