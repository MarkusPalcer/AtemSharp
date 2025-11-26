namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to acknowledge receipt of data transfer
/// </summary>
[Command("FTUA")]
[BufferSize(4)]
public partial class DataTransferAckCommand : SerializedCommand
{
    /// <summary>
    /// Transfer ID to acknowledge
    /// </summary>
    [SerializedField(0, 0)] private ushort _transferId;

    /// <summary>
    /// Transfer index to acknowledge
    /// </summary>
    [SerializedField(2, 0)] private byte _transferIndex;


    /// <summary>
    /// Create command with specified transfer acknowledgment data
    /// </summary>
    /// <param name="transferId">Transfer ID to acknowledge</param>
    /// <param name="transferIndex">Transfer index to acknowledge</param>
    public DataTransferAckCommand(ushort transferId, byte transferIndex)
    {
        TransferId = transferId;
        TransferIndex = transferIndex;
    }
}
