using AtemSharp.Enums;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command to set the lock state for a data transfer
/// </summary>
[Command("LOCK")]
public class LockStateCommand : SerializedCommand
{
    /// <summary>
    /// Index of the lock
    /// </summary>
    public ushort Index { get; set; }

    /// <summary>
    /// Whether the lock should be locked or unlocked
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// Create a new lock state command
    /// </summary>
    /// <param name="index">Index of the lock</param>
    /// <param name="locked">Whether the lock should be locked or unlocked</param>
    public LockStateCommand(ushort index, bool locked)
    {
        Index = index;
        Locked = locked;
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);
        
        writer.WriteUInt16BigEndian(Index);
        writer.WriteBoolean(Locked);
        writer.Pad(1); // Padding to reach 4 bytes total
        
        return memoryStream.ToArray();
    }
}