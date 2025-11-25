using AtemSharp.State.DisplayClock;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to set display clock state (start/stop/reset)
/// </summary>
[Command("DCSC")]
[BufferSize(4)]
public partial class DisplayClockStateSetCommand : SerializedCommand
{
    // Field will exist in the future according to TS implementation
    [SerializedField(0)]
    [NoProperty]
    private readonly byte _id = 0;

    /// <summary>
    /// Clock state to set (stopped, running, reset)
    /// </summary>
    [SerializedField(1,0)]
    private DisplayClockClockState _state;

    /// <summary>
    /// Create command with specified clock state
    /// </summary>
    /// <param name="state">Clock state to set</param>
    public DisplayClockStateSetCommand(State.DisplayClock.DisplayClock state)
    {
        _state = state.ClockState;
    }
}
