using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to send or receive data transfer content
/// This command can be both sent to and received from the ATEM device
/// </summary>
[Command("FTDa")]
public class DataTransferDataCommand : SerializedCommand, IDeserializedCommand
{
    // This command does not use the generated code because the payload does
    // not have a fixed size

    /// <summary>
    /// Transfer ID
    /// </summary>
    public required ushort TransferId { get; init; }

    /// <summary>
    /// Data content
    /// </summary>
    public required byte[] Body { get; init; } = [];

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4 + Body.Length);
        using var writer = new BinaryWriter(memoryStream);

        writer.WriteUInt16BigEndian(TransferId);
        writer.WriteUInt16BigEndian((ushort)Body.Length);
        writer.Write(Body);

        return memoryStream.ToArray();
    }

    public static DataTransferDataCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new DataTransferDataCommand
        {
            TransferId = rawCommand.ReadUInt16BigEndian(0),
            Body = rawCommand.Slice(4, rawCommand.ReadUInt16BigEndian(2)).ToArray()
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just data transport, no state changes
        // The TypeScript implementation also returns an empty array
    }
}
