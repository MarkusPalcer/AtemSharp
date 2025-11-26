using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer operation encounters an error
/// </summary>
[Command("FTDE")]
public partial class DataTransferErrorCommand : IDeserializedCommand
{

    /// <summary>
    /// Error codes that can be returned by data transfer operations
    /// </summary>
    public enum ErrorCodes : byte
    {
        /// <summary>
        /// The operation should be retried
        /// </summary>
        Retry = 1,

        /// <summary>
        /// The requested resource was not found
        /// </summary>
        NotFound = 2,

        /// <summary>
        /// The resource is not locked (maybe)
        /// </summary>
        NotLocked = 5
    }

    /// <summary>
    /// ID of the transfer that encountered an error
    /// </summary>
    [DeserializedField(0)] private ushort _transferId;

    /// <summary>
    /// The error code indicating what type of error occurred
    /// </summary>
    [DeserializedField(2)] private ErrorCodes _errorCode;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer encountered an error
        // The TypeScript implementation also returns an empty array
    }
}
