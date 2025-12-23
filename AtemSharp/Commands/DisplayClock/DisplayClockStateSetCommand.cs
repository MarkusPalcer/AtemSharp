using AtemSharp.State.DisplayClock;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
    /// Used to start, stop or reset the display clock
/// </summary>
[Command("DCSC")]
[BufferSize(4)]
public partial class DisplayClockStateSetCommand(State.DisplayClock.DisplayClock state) : SerializedCommand
{
    // Field will exist in the future according to TS implementation
    [SerializedField(0)] [NoProperty] private readonly byte _id = 0;
    [SerializedField(1, 0)] private DisplayClockClockState _state = state.ClockState;
}
