using AtemSharp.State.DisplayClock;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to set display clock state (start/stop/reset)
/// </summary>
[Command("DCSC")]
[BufferSize(4)]
public partial class DisplayClockStateSetCommand(State.DisplayClock.DisplayClock state) : SerializedCommand
{
    // Field will exist in the future according to TS implementation
    [SerializedField(0)] [NoProperty] private readonly byte _id = 0;

    /// <summary>
    /// Clock state to set
    /// </summary>
    [SerializedField(1, 0)] private DisplayClockClockState _state = state.ClockState;
}
