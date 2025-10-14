using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to set display clock state (start/stop/reset)
/// </summary>
[Command("DCSC")]
public class DisplayClockStateSetCommand : SerializedCommand
{
    private DisplayClockClockState _state;

    /// <summary>
    /// Create command with specified clock state
    /// </summary>
    /// <param name="state">Clock state to set</param>
    public DisplayClockStateSetCommand(DisplayClockClockState state)
    {
        State = state;
    }

    /// <summary>
    /// Clock state to set (stopped, running, reset)
    /// </summary>
    public DisplayClockClockState State
    {
        get => _state;
        set
        {
            _state = value;
            Flag |= 1; // Simple command with single property
        }
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);

        // Future: id at byte 0 (skip for now)
        writer.Pad(1);
        
        writer.Write((byte)State);
        
        writer.Pad(2); // Pad to buffer size of 4

        return memoryStream.ToArray();
    }
}