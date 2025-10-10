using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer lock state has been updated
/// </summary>
[Command("LKST")]
public class LockStateUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// Index of the lock that was updated
    /// </summary>
    public ushort Index { get; set; }

    /// <summary>
    /// Whether the lock is now locked or unlocked
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    /// <param name="stream">Binary stream to read from</param>
    /// <returns>Deserialized command</returns>
    public static LockStateUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);
        
        return new LockStateUpdateCommand
        {
            Index = reader.ReadUInt16BigEndian(),
            Locked = reader.ReadBoolean()
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public string[] ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a lock state was updated
        // The TypeScript implementation also returns an empty array
        return [];
    }
}