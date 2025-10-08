using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Properties for data transfer acknowledgment
/// </summary>
public class DataTransferAckProperties
{
    public ushort TransferId { get; set; }
    public byte TransferIndex { get; set; }
}

/// <summary>
/// Symmetrical command for data transfer acknowledgment
/// </summary>
public class DataTransferAckCommand : SymmetricalCommand<DataTransferAckProperties>
{
    public new static readonly string RawName = "FTUA";

    public DataTransferAckCommand(DataTransferAckProperties properties) : base(properties)
    {
    }

    public static DataTransferAckCommand Deserialize(byte[] rawCommand)
    {
        var properties = new DataTransferAckProperties
        {
            TransferId = (ushort)((rawCommand[0] << 8) | rawCommand[1]),
            TransferIndex = rawCommand[2]
        };

        return new DataTransferAckCommand(properties);
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[4];

        // Transfer ID
        buffer[0] = (byte)(Properties.TransferId >> 8);
        buffer[1] = (byte)(Properties.TransferId & 0xFF);
        
        // Transfer Index
        buffer[2] = Properties.TransferIndex;

        return buffer;
    }

    public override string[] ApplyToState(AtemState state)
    {
        // Nothing to do for acknowledgment
        return new string[0];
    }
}