using System.Drawing;
using AtemSharp.State;
using AtemSharp.State.DisplayClock;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Update command for display clock properties
/// </summary>
[Command("DCPV")]
public partial class DisplayClockPropertiesGetCommand : IDeserializedCommand
{
    /// <summary>
    /// Whether the display clock is enabled
    /// </summary>
    [DeserializedField(1)] private bool _enabled;

    /// <summary>
    /// Size of the clock display
    /// </summary>
    [DeserializedField(3)] private byte _size;

    /// <summary>
    /// Opacity of the clock display (0-255)
    /// </summary>
    [DeserializedField(5)] private byte _opacity;

    /// <summary>
    /// X position of the clock display
    /// </summary>
    [DeserializedField(6)] [ScalingFactor(1000.0)] [SerializedType(typeof(short))]
    private double _positionX;

    /// <summary>
    /// Y position of the clock display
    /// </summary>
    [DeserializedField(8)] [ScalingFactor(1000.0)] [SerializedType(typeof(short))]
    private double _positionY;

    /// <summary>
    /// Whether the clock should auto-hide
    /// </summary>
    [DeserializedField(10)] private bool _autoHide;

    /// <summary>
    /// Clock mode (countdown, countup, time of day)
    /// </summary>
    [DeserializedField(15)] private DisplayClockClockMode _clockMode;

    /// <summary>
    /// Clock state (stopped, running, reset)
    /// </summary>
    [DeserializedField(16)] private DisplayClockClockState _clockState;


    /// <summary>
    /// Starting time for countdown/countup modes
    /// </summary>
    [DeserializedField(11)] private byte _startFromHours;

    [DeserializedField(12)] private byte _startFromMinutes;
    [DeserializedField(13)] private byte _startFromSeconds;
    [DeserializedField(14)] private byte _startFromFrames;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        state.DisplayClock.Enabled = Enabled;
        state.DisplayClock.Size = Size;
        state.DisplayClock.Opacity = Opacity;
        state.DisplayClock.Location = new PointF((float)PositionX, (float)PositionY);
        state.DisplayClock.AutoHide = AutoHide;
        state.DisplayClock.StartFrom = new()
        {
            Hours = StartFromHours,
            Minutes = StartFromMinutes,
            Seconds = StartFromSeconds,
            Frames = StartFromFrames
        };
        state.DisplayClock.ClockMode = ClockMode;
        state.DisplayClock.ClockState = ClockState;
    }
}
