using AtemSharp.State;

namespace AtemSharp.Commands;

[Command("Time")]
public partial class TimeUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _hours;
    [DeserializedField(1)] private byte _minutes;
    [DeserializedField(2)] private byte _seconds;
    [DeserializedField(3)] private byte _frames;
    [DeserializedField(5)] private bool _isDropFrame;

    public void ApplyToState(AtemState state)
    {
        state.TimeCode.Hours = _hours;
        state.TimeCode.Minutes = _minutes;
        state.TimeCode.Seconds = _seconds;
        state.TimeCode.Frames = _frames;
        state.TimeCode.IsDropFrame = _isDropFrame;
    }
}
