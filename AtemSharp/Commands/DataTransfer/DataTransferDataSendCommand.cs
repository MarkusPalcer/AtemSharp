using AtemSharp.State.Info;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Used to send data transfer content
/// </summary>
[Command("FTDa")]
public class DataTransferDataSendCommand : SerializedCommand
{
    // This command does not use the generated code because the payload does
    // not have a fixed size

    public required ushort TransferId { get; set; }

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
