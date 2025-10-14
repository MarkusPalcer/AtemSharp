using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer operation is complete
/// </summary>
[Command("FTDC")]
public class DataTransferCompleteCommand : IDeserializedCommand
{
    /// <summary>
    /// ID of the transfer that completed
    /// </summary>
    public ushort TransferId { get; init; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    public static DataTransferCompleteCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new DataTransferCompleteCommand
        {
            TransferId = rawCommand.ReadUInt16BigEndian(0)
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer completed
        // The TypeScript implementation also returns an empty array
    }
}
