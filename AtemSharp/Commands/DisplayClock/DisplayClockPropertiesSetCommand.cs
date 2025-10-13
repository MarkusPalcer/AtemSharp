using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DisplayClock;

/// <summary>
/// Command to set display clock properties
/// </summary>
[Command("DCPC")]
public class DisplayClockPropertiesSetCommand : SerializedCommand
{
    private bool _enabled;
    private byte _size;
    private byte _opacity;
    private double _positionX;
    private double _positionY;
    private bool _autoHide;
    private DisplayClockTime _startFrom = new();
    private uint _startFromFrames;
    private DisplayClockClockMode _clockMode;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    public DisplayClockPropertiesSetCommand(AtemState currentState)
    {
        // If old state does not exist, set Properties (instead of backing fields) to default values,
        // so all flags are set (i.e. all values are to be applied by the ATEM)
        if (currentState.DisplayClock is null)
        {
            Enabled = false;
            Size = 0;
            Opacity = 255;
            PositionX = 0.0;
            PositionY = 0.0;
            AutoHide = false;
            StartFrom = new DisplayClockTime();
            StartFromFrames = 0;
            ClockMode = DisplayClockClockMode.Countdown;
            return;
        }

        var displayClockData = currentState.DisplayClock;

        // Initialize from current state (direct field access = no flags set)
        _enabled = displayClockData.Enabled;
        _size = displayClockData.Size;
        _opacity = displayClockData.Opacity;
        _positionX = displayClockData.PositionX;
        _positionY = displayClockData.PositionY;
        _autoHide = displayClockData.AutoHide;
        _startFrom = displayClockData.StartFrom;
        _startFromFrames = 0; // Default value for startFromFrames (this is an extension property)
        _clockMode = displayClockData.ClockMode;
    }

    /// <summary>
    /// Whether the display clock is enabled
    /// </summary>
    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            Flag |= 1 << 0;
        }
    }

    /// <summary>
    /// Size of the clock display
    /// </summary>
    public byte Size
    {
        get => _size;
        set
        {
            _size = value;
            Flag |= 1 << 1;
        }
    }

    /// <summary>
    /// Opacity of the clock display (0-255)
    /// </summary>
    public byte Opacity
    {
        get => _opacity;
        set
        {
            _opacity = value;
            Flag |= 1 << 2;
        }
    }

    /// <summary>
    /// X position of the clock display
    /// </summary>
    public double PositionX
    {
        get => _positionX;
        set
        {
            _positionX = value;
            Flag |= 1 << 3;
        }
    }

    /// <summary>
    /// Y position of the clock display
    /// </summary>
    public double PositionY
    {
        get => _positionY;
        set
        {
            _positionY = value;
            Flag |= 1 << 4;
        }
    }

    /// <summary>
    /// Whether the clock should auto-hide
    /// </summary>
    public bool AutoHide
    {
        get => _autoHide;
        set
        {
            _autoHide = value;
            Flag |= 1 << 5;
        }
    }

    /// <summary>
    /// Starting time for countdown/countup modes
    /// </summary>
    public DisplayClockTime StartFrom
    {
        get => _startFrom;
        set
        {
            _startFrom = value;
            Flag |= 1 << 6;
        }
    }

    /// <summary>
    /// Starting time as frame count (extended property)
    /// </summary>
    public uint StartFromFrames
    {
        get => _startFromFrames;
        set
        {
            _startFromFrames = value;
            Flag |= 1 << 7;
        }
    }

    /// <summary>
    /// Clock mode (countdown, countup, time of day)
    /// </summary>
    public DisplayClockClockMode ClockMode
    {
        get => _clockMode;
        set
        {
            _clockMode = value;
            Flag |= 1 << 8;
        }
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(28);
        using var writer = new BinaryWriter(memoryStream);

        // Future: id at byte 2 (skip for now)

        // Write flag as 16-bit value (matches TypeScript pattern)
        writer.WriteUInt16BigEndian((ushort)Flag);
        writer.Pad(1); // Pad to byte 3

        writer.WriteBoolean(Enabled);
        writer.Pad(1); // Pad to byte 5
        writer.Write(Size);
        writer.Pad(1); // Pad to byte 7
        writer.Write(Opacity);

        // Position values are stored as integers with 3 decimal precision (multiply by 1000)
        writer.WriteInt16BigEndian((short)Math.Round(PositionX * 1000));
        writer.WriteInt16BigEndian((short)Math.Round(PositionY * 1000));

        writer.WriteBoolean(AutoHide);

        // StartFrom time values (bytes 13-16)
        writer.Write(StartFrom.Hours);
        writer.Write(StartFrom.Minutes);
        writer.Write(StartFrom.Seconds);
        writer.Write(StartFrom.Frames);

        writer.Pad(3); // Pad to byte 20

        writer.WriteUInt32BigEndian(StartFromFrames);

        writer.Write((byte)ClockMode);

        writer.Pad(3); // Pad to final buffer size of 28

        return memoryStream.ToArray();
    }
}