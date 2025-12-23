using AtemSharp.State;
using AtemSharp.State.Info;

namespace AtemSharp.Commands.Streaming;

[Command("SRST", ProtocolVersion.V8_1_1)]
internal partial class StreamingDurationUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _hours;
    [DeserializedField(1)] private byte _minutes;
    [DeserializedField(2)] private byte _seconds;
    [DeserializedField(3)] private byte _frames;
    [DeserializedField(4)] private bool _isDropFrame;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Streaming.Duration.Hours = _hours;
        state.Streaming.Duration.Minutes = _minutes;
        state.Streaming.Duration.Seconds = _seconds;
        state.Streaming.Duration.Frames = _frames;
        state.Streaming.Duration.IsDropFrame = _isDropFrame;
    }
}
