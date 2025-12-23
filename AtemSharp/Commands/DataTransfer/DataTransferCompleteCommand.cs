using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

[Command("FTDC")]
internal partial class DataTransferCompleteCommand : IDeserializedCommand
{
    [DeserializedField(0)]
    private ushort _transferId;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer completed
    }
}
