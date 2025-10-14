using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when ATEM sends instructions to continue data upload with specific chunk parameters
/// </summary>
[Command("FTCD")]
public class DataTransferUploadContinueCommand : IDeserializedCommand
{
    /// <summary>
    /// ID of the transfer that should continue
    /// </summary>
    public ushort TransferId { get; set; }

    /// <summary>
    /// Size of each chunk to be sent in bytes
    /// </summary>
    public ushort ChunkSize { get; set; }

    /// <summary>
    /// Number of chunks to send in sequence
    /// </summary>
    public ushort ChunkCount { get; set; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    /// <param name="stream">Binary stream to read from</param>
    /// <returns>Deserialized command</returns>
    public static DataTransferUploadContinueCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        var transferId = reader.ReadUInt16BigEndian();
        reader.ReadBytes(4); // Skip 4 bytes (offsets 2-5)
        var chunkSize = reader.ReadUInt16BigEndian();
        var chunkCount = reader.ReadUInt16BigEndian();

        return new DataTransferUploadContinueCommand
        {
            TransferId = transferId,
            ChunkSize = chunkSize,
            ChunkCount = chunkCount
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just upload flow control, no state changes
        // The TypeScript implementation also returns an empty array
    }
}
