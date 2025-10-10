using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to send or receive data transfer content
/// This command can be both sent to and received from the ATEM device
/// </summary>
[Command("FTDa")]
public class DataTransferDataCommand : SerializedCommand, IDeserializedCommand
{
    /// <summary>
    /// Create command with specified transfer data
    /// </summary>
    /// <param name="transferId">Transfer ID</param>
    /// <param name="body">Data content</param>
    public DataTransferDataCommand(ushort transferId, byte[] body)
    {
        TransferId = transferId;
        Body = body ?? throw new ArgumentNullException(nameof(body));
    }

    /// <summary>
    /// Create command with default values
    /// </summary>
    public DataTransferDataCommand()
    {
        TransferId = 0;
        Body = [];
    }

    /// <summary>
    /// Transfer ID
    /// </summary>
    public ushort TransferId { get; set; }

    /// <summary>
    /// Data content
    /// </summary>
    public byte[] Body { get; set; }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as byte array</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4 + Body.Length);
        using var writer = new BinaryWriter(memoryStream);
        
        writer.WriteUInt16BigEndian(TransferId);
        writer.WriteUInt16BigEndian((ushort)Body.Length);
        writer.Write(Body);
        
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    /// <param name="stream">Binary stream to read from</param>
    /// <returns>Deserialized command</returns>
    public static DataTransferDataCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
        
        var transferId = reader.ReadUInt16BigEndian();
        var size = reader.ReadUInt16BigEndian();
        var body = reader.ReadBytes(size);
        
        return new DataTransferDataCommand(transferId, body);
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public string[] ApplyToState(AtemState state)
    {
        // Nothing to do - this is just data transport, no state changes
        // The TypeScript implementation also returns an empty array
        return [];
    }
}