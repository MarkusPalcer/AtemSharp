namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to request a data transfer upload to the ATEM device
/// </summary>
[Command("FTSD")]
[BufferSize(16)]
public partial class DataTransferUploadRequestCommand : SerializedCommand
{
    [SerializedField(0)] private ushort _transferId;

    [SerializedField(2)] private ushort _transferStoreId;

    [SerializedField(6)] private ushort _transferIndex;

    /// <summary>
    /// Size of the data to transfer
    /// </summary>
    [SerializedField(8)] private uint _size;

    /// <summary>
    /// Transfer mode
    /// Note: maybe this should be an enum, but we don't have a good description, and it shouldn't be used externally
    /// </summary>
    [SerializedField(12)] private ushort _mode;

    /// <summary>
    /// Create command with specified transfer parameters
    /// </summary>
    /// <param name="transferId">Transfer ID</param>
    /// <param name="transferStoreId">Transfer store ID</param>
    /// <param name="transferIndex">Transfer index</param>
    /// <param name="size">Size of the data to transfer</param>
    /// <param name="mode">Transfer mode</param>
    public DataTransferUploadRequestCommand(ushort transferId, ushort transferStoreId, ushort transferIndex, uint size, ushort mode)
    {
        TransferId = transferId;
        TransferStoreId = transferStoreId;
        TransferIndex = transferIndex;
        Size = size;
        Mode = mode;
    }
}
