using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

[Command("FTCD")]
internal partial class DataTransferUploadContinueCommand : IDeserializedCommand
{
    [DeserializedField(0)] private ushort _transferId;
    [DeserializedField(6)] private ushort _chunkSize;
    [DeserializedField(8)] private ushort _chunkCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just upload flow control, no state changes
        // The TypeScript implementation also returns an empty array
    }
}
