using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Update command for display clock properties
/// </summary>
[Command("DCPV")]
public class DisplayClockPropertiesGetCommand : IDeserializedCommand
{
    /// <summary>
    /// Whether the display clock is enabled
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Size of the clock display
    /// </summary>
    public byte Size { get; init; }

    /// <summary>
    /// Opacity of the clock display (0-255)
    /// </summary>
    public byte Opacity { get; init; }

    /// <summary>
    /// X position of the clock display
    /// </summary>
    public double PositionX { get; init; }

    /// <summary>
    /// Y position of the clock display
    /// </summary>
    public double PositionY { get; init; }

    /// <summary>
    /// Whether the clock should auto-hide
    /// </summary>
    public bool AutoHide { get; init; }

    /// <summary>
    /// Starting time for countdown/countup modes
    /// </summary>
    public DisplayClockTime StartFrom { get; init; } = new();

    /// <summary>
    /// Clock mode (countdown, countup, time of day)
    /// </summary>
    public DisplayClockClockMode ClockMode { get; init; }

    /// <summary>
    /// Clock state (stopped, running, reset)
    /// </summary>
    public DisplayClockClockState ClockState { get; init; }

    public static DisplayClockPropertiesGetCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new DisplayClockPropertiesGetCommand
        {
            Enabled = rawCommand.ReadBoolean(1),
            Size = rawCommand.ReadUInt8(3),
            Opacity = rawCommand.ReadUInt8(5),
            PositionX = Math.Round(rawCommand.ReadInt16BigEndian(6) / 1000.0, 3),
            PositionY = Math.Round(rawCommand.ReadInt16BigEndian(8) / 1000.0, 3),
            AutoHide = rawCommand.ReadBoolean(10),
            StartFrom = new DisplayClockTime
            {
                Hours = rawCommand.ReadUInt8(11),
                Minutes = rawCommand.ReadUInt8(12),
                Seconds = rawCommand.ReadUInt8(13),
                Frames = rawCommand.ReadUInt8(14)
            },
            ClockMode = (DisplayClockClockMode)rawCommand.ReadUInt8(15),
            ClockState = (DisplayClockClockState)rawCommand.ReadUInt8(16)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.DisplayClock ??= new State.DisplayClock();

        state.DisplayClock.Enabled = Enabled;
        state.DisplayClock.Size = Size;
        state.DisplayClock.Opacity = Opacity;
        state.DisplayClock.PositionX = PositionX;
        state.DisplayClock.PositionY = PositionY;
        state.DisplayClock.AutoHide = AutoHide;
        state.DisplayClock.StartFrom = StartFrom;
        state.DisplayClock.ClockMode = ClockMode;
        state.DisplayClock.ClockState = ClockState;
    }
}
