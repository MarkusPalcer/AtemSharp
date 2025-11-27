using AtemSharp.State.Info;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to send or receive data transfer content
/// This command can be both sent to and received from the ATEM device
/// </summary>
[Command("FTDa")]
public class DataTransferDataSendCommand : SerializedCommand
{
    // This command does not use the generated code because the payload does
    // not have a fixed size

    /// <summary>
    /// Transfer ID
    /// </summary>
    public required ushort TransferId { get; set; }

    /// <summary>
    /// Data content
    /// </summary>
    public required byte[] Body { get; set; } = [];

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4 + Body.Length];
        buffer.WriteUInt16BigEndian(TransferId, 0);
        buffer.WriteUInt16BigEndian((ushort)Body.Length, 2);
        Body.AsSpan().CopyTo(buffer.AsSpan()[4..]);

        return buffer;
    }
}
