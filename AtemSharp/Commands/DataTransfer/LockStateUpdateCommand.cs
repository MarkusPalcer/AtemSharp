using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

[Command("LKST")]
internal partial class LockStateUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _index;
    [DeserializedField(2)] private bool _locked;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a lock state was updated
        // The TypeScript implementation also returns an empty array
    }
}
