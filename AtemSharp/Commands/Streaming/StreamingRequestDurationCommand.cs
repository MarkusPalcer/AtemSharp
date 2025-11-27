using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

[Command("SRDR", ProtocolVersion.V8_1_1)]
public class StreamingRequestDurationCommand : SerializedCommand
{
    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        return [];
    }
}
