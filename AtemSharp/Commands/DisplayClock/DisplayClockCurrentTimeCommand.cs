using System.Text;
using AtemSharp.Enums;
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
    public DisplayClockTime Time { get; set; } = new();

    public static DisplayClockCurrentTimeCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        // Future: id at byte 0 (skip for now)
        reader.ReadByte();

        var hours = reader.ReadByte();
        var minutes = reader.ReadByte();
        var seconds = reader.ReadByte();
        var frames = reader.ReadByte();

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
        state.DisplayClock ??= new DisplayClockState();

        state.DisplayClock.CurrentTime = Time;
    }
}
