using AtemSharp.Enums;
using AtemSharp.Lib;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to acknowledge receipt of data transfer
/// </summary>
[Command("FTUA")]
public class DataTransferAckCommand : SerializedCommand
{
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

    /// <summary>
    /// Create command with default values
    /// </summary>
    public DataTransferAckCommand()
    {
        TransferId = 0;
        TransferIndex = 0;
    }

    /// <summary>
    /// Transfer ID to acknowledge
    /// </summary>
    public ushort TransferId { get; set; }

    /// <summary>
    /// Transfer index to acknowledge
    /// </summary>
    public byte TransferIndex { get; set; }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as byte array</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);
        
        writer.WriteUInt16BigEndian(TransferId);
        writer.Write(TransferIndex);
        writer.Pad(1); // Pad to match 4-byte buffer from TypeScript
        
        return memoryStream.ToArray();
    }
}