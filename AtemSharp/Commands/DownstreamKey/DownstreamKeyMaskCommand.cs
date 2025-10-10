using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to update downstream keyer mask properties
/// </summary>
[Command("CDsM")]
public class DownstreamKeyMaskCommand : SerializedCommand
{
    private bool _enabled;
    private double _top;
    private double _bottom;
    private double _left;
    private double _right;

    /// <summary>
    /// Downstream keyer index (0-based)
    /// </summary>
    public int DownstreamKeyerId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="downstreamKeyerId">Downstream keyer index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if downstream keyer not available</exception>
    public DownstreamKeyMaskCommand(int downstreamKeyerId, AtemState currentState)
    {
        DownstreamKeyerId = downstreamKeyerId;

        // If no video state or downstream keyer array exists, initialize with defaults
        if (currentState.Video?.DownstreamKeyers == null || 
            downstreamKeyerId >= currentState.Video.DownstreamKeyers.Length ||
            currentState.Video.DownstreamKeyers[downstreamKeyerId]?.Properties?.Mask == null)
        {
            // Set default values and flags (like TypeScript pattern)
            Enabled = false;
            Top = 0.0;
            Bottom = 0.0;
            Left = 0.0;
            Right = 0.0;
            return;
        }

        var maskProps = currentState.Video.DownstreamKeyers[downstreamKeyerId]!.Properties!.Mask;
        
        // Initialize from current state (direct field access = no flags set)
        _enabled = maskProps.Enabled;
        _top = maskProps.Top;
        _bottom = maskProps.Bottom;
        _left = maskProps.Left;
        _right = maskProps.Right;
    }

    /// <summary>
    /// Whether the mask is enabled
    /// </summary>
    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            Flag |= 1 << 0;  // MaskFlags.enabled = 1
        }
    }

    /// <summary>
    /// Top edge of the mask
    /// </summary>
    public double Top
    {
        get => _top;
        set
        {
            _top = value;
            Flag |= 1 << 1;  // MaskFlags.top = 2
        }
    }

    /// <summary>
    /// Bottom edge of the mask
    /// </summary>
    public double Bottom
    {
        get => _bottom;
        set
        {
            _bottom = value;
            Flag |= 1 << 2;  // MaskFlags.bottom = 4
        }
    }

    /// <summary>
    /// Left edge of the mask
    /// </summary>
    public double Left
    {
        get => _left;
        set
        {
            _left = value;
            Flag |= 1 << 3;  // MaskFlags.left = 8
        }
    }

    /// <summary>
    /// Right edge of the mask
    /// </summary>
    public double Right
    {
        get => _right;
        set
        {
            _right = value;
            Flag |= 1 << 4;  // MaskFlags.right = 16
        }
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(12);
        using var writer = new BinaryWriter(memoryStream);
        
        // Write flag as single byte (matching TypeScript pattern)
        writer.Write((byte)Flag);
        writer.Write((byte)DownstreamKeyerId);
        writer.WriteBoolean(Enabled);
        writer.Pad(1); // Padding byte
        
        // Write mask values as 16-bit signed integers (big-endian) using coordinate conversion
        writer.WriteInt16BigEndian(Top.CoordinateToInt16());
        writer.WriteInt16BigEndian(Bottom.CoordinateToInt16());
        writer.WriteInt16BigEndian(Left.CoordinateToInt16());
        writer.WriteInt16BigEndian(Right.CoordinateToInt16());
        
        return memoryStream.ToArray();
    }
}