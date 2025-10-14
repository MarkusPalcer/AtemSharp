using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public ushort TransferId { get; init; }

    /// <summary>
    /// Size of each chunk to be sent in bytes
    /// </summary>
    public ushort ChunkSize { get; init; }

    /// <summary>
    /// Number of chunks to send in sequence
    /// </summary>
    public ushort ChunkCount { get; init; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    public static DataTransferUploadContinueCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new DataTransferUploadContinueCommand
        {
            TransferId = rawCommand.ReadUInt16BigEndian(0),
            ChunkSize = rawCommand.ReadUInt16BigEndian(6),
            ChunkCount = rawCommand.ReadUInt16BigEndian(8)
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
