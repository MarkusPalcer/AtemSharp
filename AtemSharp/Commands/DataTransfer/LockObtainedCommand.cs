using System.Text;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DataTransfer;

/// <summary>
/// Command received when a data transfer lock has been obtained
/// </summary>
[Command("LKOB")]
public class LockObtainedCommand : IDeserializedCommand
{
    /// <summary>
    /// Index of the lock that was obtained
    /// </summary>
    public ushort Index { get; set; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    /// <param name="stream">Binary stream to read from</param>
    /// <returns>Deserialized command</returns>
    public static LockObtainedCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        return new LockObtainedCommand
        {
            Index = reader.ReadUInt16BigEndian()
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a lock was obtained
        // The TypeScript implementation also returns an empty array
    }
}
