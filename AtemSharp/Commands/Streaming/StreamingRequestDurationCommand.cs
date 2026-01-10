using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

/// <summary>
/// Used to request the streaming duration update
/// </summary>
[Command("SRDR", ProtocolVersion.V8_1_1)]
public class StreamingRequestDurationCommand : EmptyCommand
{
    internal override bool TryMergeTo(SerializedCommand other)
    {
        return other is StreamingRequestDurationCommand;
    }
}
