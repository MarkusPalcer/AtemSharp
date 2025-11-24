namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to request a data transfer upload to the ATEM device
/// </summary>
[Command("FTSD")]
[BufferSize(16)]
public partial class DataTransferUploadRequestCommand : SerializedCommand
{
    [SerializedField(0)]
    private ushort _transferId;

    [SerializedField(2)]
    private ushort _transferStoreId;

    [SerializedField(6)]
    private ushort _transferIndex;

    /// <summary>
    /// Size of the data to transfer
    /// </summary>
    [SerializedField(8)]
    private uint _size;

    /// <summary>
    /// Transfer mode
    /// Note: maybe this should be an enum, but we don't have a good description, and it shouldn't be used externally
    /// </summary>
    [SerializedField(12)]
    private ushort _mode;

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

    /// <summary>
    /// Create command with default values
    /// </summary>
    public DataTransferUploadRequestCommand()
    {
        TransferId = 0;
        TransferStoreId = 0;
        TransferIndex = 0;
        Size = 0;
        Mode = 0;
    }

    //
    // /// <summary>
    // /// Serialize command to binary stream for transmission to ATEM
    // /// </summary>
    // /// <param name="version">Protocol version</param>
    // /// <returns>Serialized command data as byte array</returns>
    // public override byte[] Serialize(ProtocolVersion version)
    // {
    //     using var memoryStream = new MemoryStream(16);
    //     using var writer = new BinaryWriter(memoryStream);
    //
    //     writer.WriteUInt16BigEndian(TransferId);       // Position 0-1
    //     writer.WriteUInt16BigEndian(TransferStoreId);  // Position 2-3
    //     writer.Pad(2);                          // Position 4-5 (padding)
    //     writer.WriteUInt16BigEndian(TransferIndex);    // Position 6-7
    //     writer.WriteUInt32BigEndian(Size);             // Position 8-11
    //     writer.WriteUInt16BigEndian(Mode);             // Position 12-13
    //     writer.Pad(2);                          // Position 14-15 (padding)
    //
    //     return memoryStream.ToArray();
    // }
}
