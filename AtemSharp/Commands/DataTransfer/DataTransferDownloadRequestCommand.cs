using AtemSharp.Enums;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to request a data transfer download from the ATEM device
/// </summary>
[Command("FTSU")]
public class DataTransferDownloadRequestCommand : SerializedCommand
{
    /// <summary>
    /// Create command with specified transfer parameters
    /// </summary>
    /// <param name="transferId">Transfer ID</param>
    /// <param name="transferStoreId">Transfer store ID</param>
    /// <param name="transferIndex">Transfer index</param>
    /// <param name="transferType">Transfer type</param>
    public DataTransferDownloadRequestCommand(ushort transferId, ushort transferStoreId, ushort transferIndex, ushort transferType)
    {
        TransferId = transferId;
        TransferStoreId = transferStoreId;
        TransferIndex = transferIndex;
        TransferType = transferType;
        Unknown2 = 0; // Default value for field not supported in TypeScript
    }

    /// <summary>
    /// Create command with all parameters including the additional unknown field
    /// </summary>
    /// <param name="transferId">Transfer ID</param>
    /// <param name="transferStoreId">Transfer store ID</param>
    /// <param name="transferIndex">Transfer index</param>
    /// <param name="transferType">Transfer type</param>
    /// <param name="unknown2">Additional unknown field (not in TypeScript implementation)</param>
    public DataTransferDownloadRequestCommand(ushort transferId, ushort transferStoreId, ushort transferIndex, ushort transferType, ushort unknown2)
    {
        TransferId = transferId;
        TransferStoreId = transferStoreId;
        TransferIndex = transferIndex;
        TransferType = transferType;
        Unknown2 = unknown2;
    }

    /// <summary>
    /// Create command with default values
    /// </summary>
    public DataTransferDownloadRequestCommand()
    {
        TransferId = 0;
        TransferStoreId = 0;
        TransferIndex = 0;
        TransferType = 0;
        Unknown2 = 0;
    }

    /// <summary>
    /// Transfer ID
    /// </summary>
    public ushort TransferId { get; set; }

    /// <summary>
    /// Transfer store ID
    /// </summary>
    public ushort TransferStoreId { get; set; }

    /// <summary>
    /// Transfer index
    /// </summary>
    public ushort TransferIndex { get; set; }

    /// <summary>
    /// Transfer type
    /// </summary>
    public ushort TransferType { get; set; }

    /// <summary>
    /// Additional unknown field (not supported in TypeScript implementation)
    /// This field exists in the wire format but is not exposed in the TypeScript API
    /// </summary>
    public ushort Unknown2 { get; set; }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as byte array</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(12);
        using var writer = new BinaryWriter(memoryStream);
        
        writer.WriteUInt16BigEndian(TransferId);         // Position 0-1
        writer.WriteUInt16BigEndian(TransferStoreId);    // Position 2-3
        writer.Pad(2);                          // Position 4-5 (padding)
        writer.WriteUInt16BigEndian(TransferIndex);      // Position 6-7
        writer.WriteUInt16BigEndian(TransferType);       // Position 8-9 (Unknown in test data)
        writer.WriteUInt16BigEndian(Unknown2);           // Position 10-11 (Unknown2 in test data)
        
        return memoryStream.ToArray();
    }
}