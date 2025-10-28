using AtemSharp.Commands;
using AtemSharp.Enums;

namespace AtemSharp.Tests.Commands.Recording;

[Command("RMDR")]
public class RecordingRequestDurationCommand : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        // No payload for this command
        return [];
    }
}
