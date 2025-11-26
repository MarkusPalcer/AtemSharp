using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer lock state has been updated
/// </summary>
[Command("LKST")]
public partial class LockStateUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Index of the lock that was updated
    /// </summary>
    [DeserializedField(0)] private ushort _index;

    /// <summary>
    /// Whether the lock is now locked or unlocked
    /// </summary>
    [DeserializedField(2)] private bool _locked;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a lock state was updated
        // The TypeScript implementation also returns an empty array
    }
}
