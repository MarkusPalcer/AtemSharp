using AtemSharp.State.Info;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to request the current display clock time
/// </summary>
[Command("DSTR")]
public class DisplayClockRequestTimeCommand : SerializedCommand
{
    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version) => new byte[4];
}
