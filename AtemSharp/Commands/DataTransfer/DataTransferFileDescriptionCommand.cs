using AtemSharp.Lib;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to send file description for data transfer operations
/// </summary>
[Command("FTFD")]
[BufferSize(212)]
public partial class DataTransferFileDescriptionCommand : SerializedCommand
{
    /// <summary>
    /// Transfer ID for the file operation
    /// </summary>
    [SerializedField(0)] private ushort _transferId;

    /// <summary>
    /// Name of the file (max 64 bytes when encoded as UTF-8)
    /// </summary>
    [CustomSerialization]
    private string? _name;

    /// <summary>
    /// Description of the file (max 128 bytes when encoded as UTF-8)
    /// </summary>
    [CustomSerialization]
    private string? _description;

    /// <summary>
    /// Base64-encoded hash of the file (16 bytes when decoded)
    /// </summary>
    [CustomSerialization]
    private string _fileHash = string.Empty;

    /// <summary>
    /// Create command with specified file description data
    /// </summary>
    /// <param name="transferId">Transfer ID for the file operation</param>
    /// <param name="name">Name of the file (max 64 bytes when encoded as UTF-8)</param>
    /// <param name="description">Description of the file (max 128 bytes when encoded as UTF-8)</param>
    /// <param name="fileHash">Base64-encoded hash of the file (16 bytes when decoded)</param>
    public DataTransferFileDescriptionCommand(ushort transferId, string? name, string? description, string fileHash)
    {
        TransferId = transferId;
        Name = name;
        Description = description;
        FileHash = fileHash;
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(Name ?? string.Empty, 2, 64);
        buffer.WriteString(Description ?? string.Empty, 66, 128);

        try
        {
            buffer.WriteBlob(Convert.FromBase64String(FileHash), 194, 16);
        }
        catch (FormatException)
        {
            // If FileHash is not valid base64, leave as zeros
        }
    }
}
