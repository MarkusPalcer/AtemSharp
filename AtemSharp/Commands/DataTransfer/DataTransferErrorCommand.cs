using AtemSharp.Enums;
using AtemSharp.Enums.DataTransfer;
using AtemSharp.Lib;
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
    public ushort TransferId { get; init; }

    /// <summary>
    /// The error code indicating what type of error occurred
    /// </summary>
    public ErrorCode ErrorCode { get; init; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    public static DataTransferErrorCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new DataTransferErrorCommand
        {
            TransferId = rawCommand.ReadUInt16BigEndian(0),
            ErrorCode = (ErrorCode)rawCommand.ReadUInt8(2)
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer encountered an error
        // The TypeScript implementation also returns an empty array
    }
}
