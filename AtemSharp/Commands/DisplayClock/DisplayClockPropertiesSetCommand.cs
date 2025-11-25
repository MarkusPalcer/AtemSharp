using AtemSharp.Lib;
using AtemSharp.State;
using AtemSharp.State.DisplayClock;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to set display clock properties
/// </summary>
[Command("DCPC")]
[BufferSize(28)]
public partial class DisplayClockPropertiesSetCommand : SerializedCommand
{
    /// <summary>
    /// Whether the display clock is enabled
    /// </summary>
    [SerializedField(3,0)]
    private bool _enabled;

    /// <summary>
    /// Size of the clock display
    /// </summary>
    [SerializedField(5,1)]
    private byte _size;

    /// <summary>
    /// Opacity of the clock display (0-255)
    /// </summary>
    [SerializedField(7,2)]
    private byte _opacity;

    /// <summary>
    /// X position of the clock display
    /// </summary>
    [SerializedField(8,3)]
    [ScalingFactor(1000.0)]
    [SerializedType(typeof(short))]
    private double _positionX;

    /// <summary>
    /// Y position of the clock display
    /// </summary>
    [SerializedField(10,4)]
    [ScalingFactor(1000.0)]
    [SerializedType(typeof(short))]
    private double _positionY;

    /// <summary>
    /// Whether the clock should auto-hide
    /// </summary>
    [SerializedField(12,5)]
    private bool _autoHide;

    /// <summary>
    /// Starting time for countdown/countup modes
    /// </summary>
    [CustomSerialization(6)]
    private DisplayClockTime _startFrom;

    /// <summary>
    /// Starting time as frame count (extended property)
    /// </summary>
    [SerializedField(20,7)]
    private uint _startFromFrames;

    /// <summary>
    /// Clock mode (countdown, countup, time of day)
    /// </summary>
    [SerializedField(24,8)]
    private DisplayClockClockMode _clockMode;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    public DisplayClockPropertiesSetCommand(State.DisplayClock.DisplayClock currentState)
    {
        _enabled = currentState.Enabled;
        _size = currentState.Size;
        _opacity = currentState.Opacity;
        _positionX = currentState.PositionX;
        _positionY = currentState.PositionY;
        _autoHide = currentState.AutoHide;
        _startFrom = currentState.StartFrom;
        _startFromFrames = 0; // Default value for startFromFrames (this is an extension property)
        _clockMode = currentState.ClockMode;
    }

    private void SerializeInternal(byte[] buffer)
    {
        buffer.WriteUInt16BigEndian((ushort)Flag,0);
        buffer.WriteUInt8(StartFrom.Hours, 13);
        buffer.WriteUInt8(StartFrom.Minutes, 14);
        buffer.WriteUInt8(StartFrom.Seconds, 15);
        buffer.WriteUInt8(StartFrom.Frames, 16);
    }
}
