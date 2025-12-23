using System.Drawing;
using AtemSharp.State.DisplayClock;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Used to set display clock properties
/// </summary>
[Command("DCPC")]
[BufferSize(28)]
public partial class DisplayClockPropertiesSetCommand(State.DisplayClock.DisplayClock currentState) : SerializedCommand
{
    [SerializedField(3, 0)] private bool _enabled = currentState.Enabled;
    [SerializedField(5, 1)] private byte _size = currentState.Size;
    [SerializedField(7, 2)] private byte _opacity = currentState.Opacity;

    [SerializedField(8, 3)] [ScalingFactor(1000.0)] [SerializedType(typeof(short))]
    private double _positionX = currentState.Location.X;

    [SerializedField(10, 4)] [ScalingFactor(1000.0)] [SerializedType(typeof(short))]
    private double _positionY = currentState.Location.Y;

    [SerializedField(12, 5)] private bool _autoHide = currentState.AutoHide;

    [CustomSerialization(6)] private DisplayClockTime _startFrom = currentState.StartFrom;

    [SerializedField(20, 7)] private uint _startFromFrames;

    [SerializedField(24, 8)] private DisplayClockClockMode _clockMode = currentState.ClockMode;

    public PointF Location
    {
        get => new((float)_positionX, (float)_positionY);
        set
        {
            PositionX = value.X;
            PositionY = value.Y;
        }
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag, 0);
        buffer.WriteUInt8(StartFrom.Hours, 13);
        buffer.WriteUInt8(StartFrom.Minutes, 14);
        buffer.WriteUInt8(StartFrom.Seconds, 15);
        buffer.WriteUInt8(StartFrom.Frames, 16);
    }
}
