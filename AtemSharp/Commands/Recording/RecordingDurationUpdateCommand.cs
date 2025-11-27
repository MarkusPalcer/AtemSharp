using AtemSharp.State;

namespace AtemSharp.Commands.Recording;

[Command("RTMR")]
public partial class RecordingDurationUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _hours;

    [DeserializedField(1)] private byte _minutes;

    [DeserializedField(2)] private byte _seconds;

    [DeserializedField(3)] private byte _frames;

    [DeserializedField(4)] private bool _isDropFrame;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.Recording.Duration.Hours = _hours;
        state.Recording.Duration.Minutes = _minutes;
        state.Recording.Duration.Seconds = _seconds;
        state.Recording.Duration.Frames = _frames;
        state.Recording.Duration.IsDropFrame = _isDropFrame;
    }
}
