using System.Text;
using AtemSharp.Enums.DataTransfer;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer operation encounters an error
/// </summary>
[Command("FTDE")]
public class DataTransferErrorCommand : IDeserializedCommand
{
    /// <summary>
    /// ID of the transfer that encountered an error
    /// </summary>
    public ushort TransferId { get; set; }

    /// <summary>
    /// The error code indicating what type of error occurred
    /// </summary>
    public ErrorCode ErrorCode { get; set; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    /// <param name="stream">Binary stream to read from</param>
    /// <returns>Deserialized command</returns>
    public static DataTransferErrorCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
        
        return new DataTransferErrorCommand
        {
            TransferId = reader.ReadUInt16BigEndian(),
            ErrorCode = (ErrorCode)reader.ReadByte()
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public string[] ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer encountered an error
        // The TypeScript implementation also returns an empty array
        return [];
    }
}