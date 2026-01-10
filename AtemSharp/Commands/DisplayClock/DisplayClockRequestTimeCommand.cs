using AtemSharp.State.Info;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Used to request the current display clock time
/// </summary>
[Command("DSTR")]
public class DisplayClockRequestTimeCommand : SerializedCommand
{
    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version) => new byte[4];

    internal override bool TryMergeTo(SerializedCommand other)
    {
        return other is DisplayClockRequestTimeCommand;
    }
}
