using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer operation is complete
/// </summary>
[Command("FTDC")]
public partial class DataTransferCompleteCommand : IDeserializedCommand
{
    /// <summary>
    /// ID of the transfer that completed
    /// </summary>
    [DeserializedField(0)]
    private ushort _transferId;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a transfer completed
        // The TypeScript implementation also returns an empty array
    }
}
