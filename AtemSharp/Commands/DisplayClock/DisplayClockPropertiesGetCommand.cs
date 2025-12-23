using System.Drawing;
using AtemSharp.State;
using AtemSharp.State.DisplayClock;

namespace AtemSharp.Commands.DisplayClock;

[Command("DCPV")]
internal partial class DisplayClockPropertiesGetCommand : IDeserializedCommand
{
    [DeserializedField(1)] private bool _enabled;
    [DeserializedField(3)] private byte _size;
    [DeserializedField(5)] private byte _opacity;

    [DeserializedField(6)] [ScalingFactor(1000.0)] [SerializedType(typeof(short))]
    private double _positionX;

    [DeserializedField(8)] [ScalingFactor(1000.0)] [SerializedType(typeof(short))]
    private double _positionY;

    [DeserializedField(10)] private bool _autoHide;
    [DeserializedField(15)] private DisplayClockClockMode _clockMode;
    [DeserializedField(16)] private DisplayClockClockState _clockState;

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
