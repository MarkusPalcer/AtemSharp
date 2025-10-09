using System.Text;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer operation is complete
/// </summary>
[Command("FTDC")]
public class DataTransferCompleteCommand : DeserializedCommand
{
    /// <summary>
    /// ID of the transfer that completed
    /// </summary>
    public ushort TransferId { get; set; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    /// <param name="stream">Binary stream to read from</param>
    /// <returns>Deserialized command</returns>
    public static DataTransferCompleteCommand Deserialize(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
        
        return new DataTransferCompleteCommand
        {
            TransferId = SerializationExtensions.ReadUInt16(reader)
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public override string[] ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer completed
        // The TypeScript implementation also returns an empty array
        return [];
    }
}