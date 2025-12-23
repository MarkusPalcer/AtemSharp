using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

[Command("LKOB")]
internal partial class LockObtainedCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _index;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a lock was obtained
        // The TypeScript implementation also returns an empty array
    }
}
