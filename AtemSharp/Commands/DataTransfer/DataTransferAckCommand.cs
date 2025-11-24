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

    // public override byte[] Serialize(ProtocolVersion version)
    // {
    //     using var memoryStream = new MemoryStream(4);
    //     using var writer = new BinaryWriter(memoryStream);
    //
    //     writer.WriteUInt16BigEndian(TransferId);
    //     writer.Write(TransferIndex);
    //     writer.Pad(1); // Pad to match 4-byte buffer from TypeScript
    //
    //     return memoryStream.ToArray();
    // }
}
