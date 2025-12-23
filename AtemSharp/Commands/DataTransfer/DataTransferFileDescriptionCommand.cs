namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Used to send file description for data transfer operations
/// </summary>
[Command("FTFD")]
[BufferSize(212)]
public partial class DataTransferFileDescriptionCommand(ushort transferId, string name, string description, string fileHash)
    : SerializedCommand
{
    [SerializedField(0)] private ushort _transferId = transferId;
    [CustomSerialization] private string _name = name;
    [CustomSerialization] private string _description = description;
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
