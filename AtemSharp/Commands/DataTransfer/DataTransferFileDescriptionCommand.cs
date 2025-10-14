using AtemSharp.Enums;
using System.Text;
using AtemSharp.Lib;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to send file description for data transfer operations
/// </summary>
[Command("FTFD")]
public class DataTransferFileDescriptionCommand : SerializedCommand
{
    private ushort _transferId;
    private string? _name;
    private string? _description;
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

    /// <summary>
    /// Create command with default values
    /// </summary>
    public DataTransferFileDescriptionCommand()
    {
        _transferId = 0;
        _name = null;
        _description = null;
        _fileHash = string.Empty;
    }

    /// <summary>
    /// Transfer ID for the file operation
    /// </summary>
    public ushort TransferId
    {
        get => _transferId;
        set
        {
            _transferId = value;
            Flag |= 1 << 0;
        }
    }

    /// <summary>
    /// Name of the file (max 64 bytes when encoded as UTF-8)
    /// </summary>
    public string? Name
    {
        get => _name;
        set
        {
            _name = value;
            Flag |= 1 << 1;
        }
    }

    /// <summary>
    /// Description of the file (max 128 bytes when encoded as UTF-8)
    /// </summary>
    public string? Description
    {
        get => _description;
        set
        {
            _description = value;
            Flag |= 1 << 2;
        }
    }

    /// <summary>
    /// Base64-encoded hash of the file (16 bytes when decoded)
    /// </summary>
    public string FileHash
    {
        get => _fileHash;
        set
        {
            _fileHash = value;
            Flag |= 1 << 3;
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data as byte array</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(212);
        using var writer = new BinaryWriter(memoryStream);

        // Write transfer ID (2 bytes)
        writer.WriteUInt16BigEndian(TransferId);

        // Write name (64 bytes, null-terminated UTF-8)
        var nameBytes = new byte[64];
        if (!string.IsNullOrEmpty(Name))
        {
            var encodedName = Encoding.UTF8.GetBytes(Name);
            var copyLength = Math.Min(encodedName.Length, 63); // Leave space for null terminator
            Array.Copy(encodedName, nameBytes, copyLength);
        }
        writer.Write(nameBytes);

        // Write description (128 bytes, null-terminated UTF-8)
        var descriptionBytes = new byte[128];
        if (!string.IsNullOrEmpty(Description))
        {
            var encodedDescription = Encoding.UTF8.GetBytes(Description);
            var copyLength = Math.Min(encodedDescription.Length, 127); // Leave space for null terminator
            Array.Copy(encodedDescription, descriptionBytes, copyLength);
        }
        writer.Write(descriptionBytes);

        // Write file hash (16 bytes from base64)
        var hashBytes = new byte[16];
        if (!string.IsNullOrEmpty(FileHash))
        {
            try
            {
                var decodedHash = Convert.FromBase64String(FileHash);
                var copyLength = Math.Min(decodedHash.Length, 16);
                Array.Copy(decodedHash, hashBytes, copyLength);
            }
            catch (FormatException)
            {
                // If FileHash is not valid base64, leave as zeros
            }
        }
        writer.Write(hashBytes);

        // Write padding to ensure total length is 212 bytes
        var currentLength = memoryStream.Position;
        var paddingLength = 212 - currentLength;
        if (paddingLength > 0)
        {
            writer.Pad((uint)paddingLength);
        }

        return memoryStream.ToArray();
    }
}