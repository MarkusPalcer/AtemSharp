namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to send file description for data transfer operations
/// </summary>
[Command("FTFD")]
[BufferSize(212)]
public partial class DataTransferFileDescriptionCommand(ushort transferId, string name, string description, string fileHash)
    : SerializedCommand
{
    /// <summary>
    /// Transfer ID for the file operation
    /// </summary>
    [SerializedField(0)] private ushort _transferId = transferId;

    /// <summary>
    /// Name of the file (max 64 bytes when encoded as UTF-8)
    /// </summary>
    [CustomSerialization] private string _name = name;

    /// <summary>
    /// Description of the file (max 128 bytes when encoded as UTF-8)
    /// </summary>
    [CustomSerialization] private string _description = description;

    /// <summary>
    /// Base64-encoded hash of the file (16 bytes when decoded)
    /// </summary>
    [CustomSerialization] private string _fileHash = fileHash;

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteString(Name, 2, 64);
        buffer.WriteString(Description, 66, 128);

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
