using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.DownstreamKey;

/// <summary>
/// Command to update downstream keyer general properties (pre-multiply, clip, gain, invert)
/// </summary>
[Command("CDsG")]
public class DownstreamKeyGeneralCommand : SerializedCommand
{
    private bool _preMultiply;
    private double _clip;
    private double _gain;
    private bool _invert;

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
    public DownstreamKeyGeneralCommand(int downstreamKeyerId, AtemState currentState)
    {
        DownstreamKeyerId = downstreamKeyerId;

        if (downstreamKeyerId >= currentState.Video.DownstreamKeyers.Length)
        {
            throw new IndexOutOfRangeException("DownstreamKeyerId is out of range");
        }

        var dsk = currentState.Video.DownstreamKeyers[downstreamKeyerId];
        var dskProps = dsk.Properties!;

        // Initialize from current state (direct field access = no flags set)
        _preMultiply = dskProps.PreMultiply;
        _clip = dskProps.Clip;
        _gain = dskProps.Gain;
        _invert = dskProps.Invert;
    }

    /// <summary>
    /// Whether to pre-multiply the key signal
    /// </summary>
    public bool PreMultiply
    {
        get => _preMultiply;
        set
        {
            _preMultiply = value;
            Flag |= 1 << 0;  // MaskFlags.preMultiply = 1
        }
    }

    /// <summary>
    /// Clip threshold (0.0 to 100.0 percent)
    /// </summary>
    public double Clip
    {
        get => _clip;
        set
        {
            if (value < 0.0 || value > 100.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Clip must be between 0.0 and 100.0");
            }
            _clip = value;
            Flag |= 1 << 1;  // MaskFlags.clip = 2
        }
    }

    /// <summary>
    /// Gain value (0.0 to 100.0 percent)
    /// </summary>
    public double Gain
    {
        get => _gain;
        set
        {
            if (value < 0.0 || value > 100.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Gain must be between 0.0 and 100.0");
            }
            _gain = value;
            Flag |= 1 << 2;  // MaskFlags.gain = 4
        }
    }

    /// <summary>
    /// Whether to invert the key signal
    /// </summary>
    public bool Invert
    {
        get => _invert;
        set
        {
            _invert = value;
            Flag |= 1 << 3;  // MaskFlags.invert = 8
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(12);
        using var writer = new BinaryWriter(memoryStream);

        // Write flag as single byte (matching TypeScript pattern)
        writer.Write((byte)Flag);
        writer.Write((byte)DownstreamKeyerId);
        writer.WriteBoolean(PreMultiply);
        writer.Pad(1); // Align to 2-byte boundary

        // Convert percentages to fixed-point integers (multiplied by 10)
        // TypeScript: Math.round(v * 10)
        writer.WriteInt16BigEndian((short)Math.Round(Clip * 10));
        writer.WriteInt16BigEndian((short)Math.Round(Gain * 10));
        writer.WriteBoolean(Invert);
        writer.Pad(3); // Pad to total 12 bytes

        return memoryStream.ToArray();
    }
}
