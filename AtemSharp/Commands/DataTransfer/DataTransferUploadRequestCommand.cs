using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Properties for data transfer upload request
/// </summary>
public class DataTransferUploadRequestProperties
{
    public ushort TransferId { get; set; }
    public ushort TransferStoreId { get; set; }
    public ushort TransferIndex { get; set; }
    public uint Size { get; set; }
    public ushort Mode { get; set; } // Note: maybe this should be an enum, but we don't have a good description
}

/// <summary>
/// Command to request a data transfer upload
/// </summary>
public class DataTransferUploadRequestCommand : BasicWritableCommand<DataTransferUploadRequestProperties>
{
    public new static readonly string RawName = "FTSD";

    public DataTransferUploadRequestCommand(DataTransferUploadRequestProperties properties) : base(properties)
    {
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[16];
        
        // Transfer ID
        buffer[0] = (byte)(Properties.TransferId >> 8);
        buffer[1] = (byte)(Properties.TransferId & 0xFF);
        
        // Transfer Store ID
        buffer[2] = (byte)(Properties.TransferStoreId >> 8);
        buffer[3] = (byte)(Properties.TransferStoreId & 0xFF);
        
        // Transfer Index
        buffer[6] = (byte)(Properties.TransferIndex >> 8);
        buffer[7] = (byte)(Properties.TransferIndex & 0xFF);
        
        // Size
        buffer[8] = (byte)(Properties.Size >> 24);
        buffer[9] = (byte)((Properties.Size >> 16) & 0xFF);
        buffer[10] = (byte)((Properties.Size >> 8) & 0xFF);
        buffer[11] = (byte)(Properties.Size & 0xFF);
        
        // Mode
        buffer[12] = (byte)(Properties.Mode >> 8);
        buffer[13] = (byte)(Properties.Mode & 0xFF);
        
        return buffer;
    }
}