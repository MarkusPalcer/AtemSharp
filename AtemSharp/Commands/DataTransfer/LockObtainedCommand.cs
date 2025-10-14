using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public ushort Index { get; init; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    public static LockObtainedCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new LockObtainedCommand
        {
            Index = rawCommand.ReadUInt16BigEndian(0)
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
