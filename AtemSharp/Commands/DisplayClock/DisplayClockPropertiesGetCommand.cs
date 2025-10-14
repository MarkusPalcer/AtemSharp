using System.Text;
using AtemSharp.Enums;
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
    public bool Enabled { get; set; }

    /// <summary>
    /// Size of the clock display
    /// </summary>
    public byte Size { get; set; }

    /// <summary>
    /// Opacity of the clock display (0-255)
    /// </summary>
    public byte Opacity { get; set; }

    /// <summary>
    /// X position of the clock display
    /// </summary>
    public double PositionX { get; set; }

    /// <summary>
    /// Y position of the clock display
    /// </summary>
    public double PositionY { get; set; }

    /// <summary>
    /// Whether the clock should auto-hide
    /// </summary>
    public bool AutoHide { get; set; }

    /// <summary>
    /// Starting time for countdown/countup modes
    /// </summary>
    public DisplayClockTime StartFrom { get; set; } = new();

    /// <summary>
    /// Clock mode (countdown, countup, time of day)
    /// </summary>
    public DisplayClockClockMode ClockMode { get; set; }

    /// <summary>
    /// Clock state (stopped, running, reset)
    /// </summary>
    public DisplayClockClockState ClockState { get; set; }

    public static DisplayClockPropertiesGetCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

        // Future: id at byte 0 (skip for now)
        reader.ReadByte();

        var enabled = reader.ReadBoolean();  // byte 1
        reader.ReadByte(); // Skip padding at byte 2
        var size = reader.ReadByte();          // byte 3
        reader.ReadByte(); // Skip padding at byte 4
        var opacity = reader.ReadByte();       // byte 5
        var positionX = reader.ReadInt16BigEndian();  // bytes 6-7
        var positionY = reader.ReadInt16BigEndian();  // bytes 8-9
        var autoHide = reader.ReadBoolean(); // byte 10

        var startFromHours = reader.ReadByte();   // byte 11
        var startFromMinutes = reader.ReadByte(); // byte 12
        var startFromSeconds = reader.ReadByte(); // byte 13
        var startFromFrames = reader.ReadByte();  // byte 14

        var clockMode = (DisplayClockClockMode)reader.ReadByte();  // byte 15
        var clockState = (DisplayClockClockState)reader.ReadByte(); // byte 16

        return new DisplayClockPropertiesGetCommand
        {
            Enabled = enabled,
            Size = size,
            Opacity = opacity,
            PositionX = Math.Round(positionX / 1000.0, 3),
            PositionY = Math.Round(positionY / 1000.0, 3),
            AutoHide = autoHide,
            StartFrom = new DisplayClockTime
            {
                Hours = startFromHours,
                Minutes = startFromMinutes,
                Seconds = startFromSeconds,
                Frames = startFromFrames
            },
            ClockMode = clockMode,
            ClockState = clockState
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.DisplayClock ??= new DisplayClockState();

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
