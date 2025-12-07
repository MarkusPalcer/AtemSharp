using AtemSharp.State;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Update command for display clock current time
/// </summary>
[Command("DSTV")]
public partial class DisplayClockCurrentTimeCommand : IDeserializedCommand
{
    /// <summary>
    /// Hours (0-23)
    /// </summary>
    [DeserializedField(1)] private byte _hours;

    /// <summary>
    /// Minutes (0-59)
    /// </summary>
    [DeserializedField(2)] private byte _minutes;

    /// <summary>
    /// Seconds (0-59)
    /// </summary>
    [DeserializedField(3)] private byte _seconds;

    /// <summary>
    /// Frames (0-59, depends on frame rate)
    /// </summary>
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
