using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to request the current display clock time
/// </summary>
[Command("DSTR")]
public class DisplayClockRequestTimeCommand : SerializedCommand
{
    /// <summary>
    /// Create command to request display clock time
    /// </summary>
    public DisplayClockRequestTimeCommand()
    {
        // No properties to set for this command
        Flag = 0;
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        // Future: id at byte 0 (skip for now)
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);

        writer.Pad(4); // Entire buffer is padding

        return memoryStream.ToArray();
    }
}