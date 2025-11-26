using AtemSharp.State.Info;

namespace AtemSharp.Commands.Recording;

[Command("RMDR")]
public class RecordingRequestDurationCommand : SerializedCommand
{
    public override byte[] Serialize(ProtocolVersion version)
    {
        // No payload for this command
        return [];
    }
}
