using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when ATEM sends instructions to continue data upload with specific chunk parameters
/// </summary>
[Command("FTCD")]
public partial class DataTransferUploadContinueCommand : IDeserializedCommand
{
    /// <summary>
    /// ID of the transfer that should continue
    /// </summary>
    [DeserializedField(0)]
    private ushort _transferId;

    /// <summary>
    /// Size of each chunk to be sent in bytes
    /// </summary>
    [DeserializedField(6)]
    private ushort _chunkSize;

    /// <summary>
    /// Number of chunks to send in sequence
    /// </summary>
    [DeserializedField(8)]
    private ushort _chunkCount;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just upload flow control, no state changes
        // The TypeScript implementation also returns an empty array
    }
}
