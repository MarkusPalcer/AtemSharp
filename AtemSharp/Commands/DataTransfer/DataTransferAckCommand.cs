namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Used to acknowledge receipt of data transfer
/// </summary>
[Command("FTUA")]
[BufferSize(4)]
public partial class DataTransferAckCommand(ushort transferId, byte transferIndex) : SerializedCommand
{
    [SerializedField(0, 0)] private ushort _transferId = transferId;

    [SerializedField(2, 0)] private byte _transferIndex = transferIndex;
}
