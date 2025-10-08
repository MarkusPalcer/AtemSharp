using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Properties for data transfer file description
/// </summary>
public class DataTransferFileDescriptionProperties
{
    public ushort TransferId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string FileHash { get; set; } = string.Empty; // Base64 encoded hash
}

/// <summary>
/// Command to set file description for data transfer
/// </summary>
public class DataTransferFileDescriptionCommand : BasicWritableCommand<DataTransferFileDescriptionProperties>
{
    public new static readonly string RawName = "FTFD";

    public DataTransferFileDescriptionCommand(DataTransferFileDescriptionProperties properties) : base(properties)
    {
    }

    public override byte[] Serialize(ProtocolVersion version)
    {
        var buffer = new byte[212];
        
        // Transfer ID
        buffer[0] = (byte)(Properties.TransferId >> 8);
        buffer[1] = (byte)(Properties.TransferId & 0xFF);
        
        // Name (64 bytes starting at offset 2)
        if (!string.IsNullOrEmpty(Properties.Name))
        {
            var nameBytes = Encoding.UTF8.GetBytes(Properties.Name);
            var nameLength = Math.Min(nameBytes.Length, 64);
            Array.Copy(nameBytes, 0, buffer, 2, nameLength);
        }
        
        // Description (128 bytes starting at offset 66)
        if (!string.IsNullOrEmpty(Properties.Description))
        {
            var descBytes = Encoding.UTF8.GetBytes(Properties.Description);
            var descLength = Math.Min(descBytes.Length, 128);
            Array.Copy(descBytes, 0, buffer, 66, descLength);
        }
        
        // File hash (16 bytes starting at offset 194)
        if (!string.IsNullOrEmpty(Properties.FileHash))
        {
            try
            {
                var hashBytes = Convert.FromBase64String(Properties.FileHash);
                var hashLength = Math.Min(hashBytes.Length, 16);
                Array.Copy(hashBytes, 0, buffer, 194, hashLength);
            }
            catch (FormatException)
            {
                // Invalid base64, leave hash bytes as zeros
            }
        }
        
        return buffer;
    }
}