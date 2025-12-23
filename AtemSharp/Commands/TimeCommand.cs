using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Used to set the current time
/// </summary>
[Command("Time")]
[BufferSize(8)]
public partial class TimeCommand(AtemState state) : SerializedCommand
{
    [SerializedField(0)] private byte _hours = state.TimeCode.Hours;
    [SerializedField(1)] private byte _minutes = state.TimeCode.Minutes;
    [SerializedField(2)] private byte _seconds = state.TimeCode.Seconds;
    [SerializedField(3)] private byte _frames = state.TimeCode.Frames;
    [SerializedField(5)] private bool _isDropFrame = state.TimeCode.IsDropFrame;
}
