using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Update command for display clock current time
/// </summary>
[Command("DSTV")]
public class DisplayClockCurrentTimeCommand : IDeserializedCommand
{
    /// <summary>
    /// Current display clock time
    /// </summary>
    public DisplayClockTime Time { get; init; } = new();

    public static DisplayClockCurrentTimeCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        var hours = rawCommand.ReadUInt8(1);
        var minutes = rawCommand.ReadUInt8(2);
        var seconds = rawCommand.ReadUInt8(3);
        var frames = rawCommand.ReadUInt8(4);

        return new DisplayClockCurrentTimeCommand
        {
            Time = new DisplayClockTime
            {
                Hours = hours,
                Minutes = minutes,
                Seconds = seconds,
                Frames = frames
            }
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.DisplayClock ??= new State.DisplayClock();

        state.DisplayClock.CurrentTime = Time;
    }
}
