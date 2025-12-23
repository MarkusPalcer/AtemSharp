using AtemSharp.State;

namespace AtemSharp.Commands.DisplayClock;

[Command("DSTV")]
internal partial class DisplayClockCurrentTimeCommand : IDeserializedCommand
{
    [DeserializedField(1)] private byte _hours;
    [DeserializedField(2)] private byte _minutes;
    [DeserializedField(3)] private byte _seconds;
    [DeserializedField(4)] private byte _frames;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.DisplayClock.CurrentTime.Hours = _hours;
        state.DisplayClock.CurrentTime.Minutes = _minutes;
        state.DisplayClock.CurrentTime.Seconds = _seconds;
        state.DisplayClock.CurrentTime.Frames = _frames;
    }
}
