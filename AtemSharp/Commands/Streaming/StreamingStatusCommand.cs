using AtemSharp.State;
using AtemSharp.State.Info;
using AtemSharp.State.Streaming;

namespace AtemSharp.Commands.Streaming;

[Command("StrR", ProtocolVersion.V8_1_1)]
[BufferSize(4)]
public partial class StreamingStatusCommand(AtemState state) : SerializedCommand
{
    [SerializedField(0)]
    private bool _isStreaming = state.Streaming.Status != StreamingStatus.Idle;
}
