using AtemSharp.Enums.DataTransfer;
using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer operation encounters an error
/// </summary>
[Command("FTDE")]
public partial class DataTransferErrorCommand : IDeserializedCommand
{
    /// <summary>
    /// ID of the transfer that encountered an error
    /// </summary>
    [DeserializedField(0)]
    private ushort _transferId;

    /// <summary>
    /// The error code indicating what type of error occurred
    /// </summary>
    [DeserializedField(2)]
    private ErrorCode _errorCode;



    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer encountered an error
        // The TypeScript implementation also returns an empty array
    }
}
