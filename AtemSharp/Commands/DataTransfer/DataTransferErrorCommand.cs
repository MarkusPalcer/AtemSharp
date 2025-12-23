using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

[Command("FTDE")]
internal partial class DataTransferErrorCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _transferId;

    [DeserializedField(2)] private ErrorCodes _errorCode;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer encountered an error
        // The TypeScript implementation also returns an empty array
    }
}
