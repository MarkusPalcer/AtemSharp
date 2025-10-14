using AtemSharp.Enums;
using AtemSharp.Lib;
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
    public ushort Index { get; init; }

    /// <summary>
    /// Whether the lock is now locked or unlocked
    /// </summary>
    public bool Locked { get; init; }

    /// <summary>
    /// Deserialize binary data into command
    /// </summary>
    public static LockStateUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new LockStateUpdateCommand
        {
            Index = rawCommand.ReadUInt16BigEndian(0),
            Locked = rawCommand.ReadBoolean(2)
        };
    }

    /// <summary>
    /// Apply this command to the ATEM state
    /// </summary>
    /// <param name="state">ATEM state to modify</param>
    /// <returns>List of state paths that were changed (empty for this command)</returns>
    public void ApplyToState(AtemState state)
    {
        // Nothing to do - this is just a notification that a lock state was updated
        // The TypeScript implementation also returns an empty array
    }
}
