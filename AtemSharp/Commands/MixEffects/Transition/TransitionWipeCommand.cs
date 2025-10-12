using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.MixEffects.Transition;

/// <summary>
/// Command to set wipe transition settings for a mix effect
/// </summary>
[Command("CTWp")]
public class TransitionWipeCommand : SerializedCommand
{
    private int _rate;
    private int _pattern;
    private double _borderWidth;
    private int _borderInput;
    private double _symmetry;
    private double _borderSoftness;
    private double _xPosition;
    private double _yPosition;
    private bool _reverseDirection;
    private bool _flipFlop;

    /// <summary>
    /// Mix effect index (0-based)
    /// </summary>
    public int MixEffectId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="mixEffectId">Mix effect index (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if mix effect not available</exception>
    public TransitionWipeCommand(int mixEffectId, AtemState currentState)
    {
        MixEffectId = mixEffectId;

        // If no video state or mix effect array exists, initialize with defaults
        if (!currentState.Video.MixEffects.TryGetValue(mixEffectId, out var mixEffect) ||
            mixEffect.TransitionSettings?.Wipe == null)
        {
            // Set default values and flags (like TypeScript pattern)
            Rate = 25;          // Default rate
            Pattern = 0;        // Default pattern
            BorderWidth = 0;    // Default border width
            BorderInput = 0;    // Default border input
            Symmetry = 0;       // Default symmetry
            BorderSoftness = 0; // Default border softness
            XPosition = 0;      // Default X position
            YPosition = 0;      // Default Y position
            ReverseDirection = false; // Default reverse direction
            FlipFlop = false;   // Default flip flop
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _rate = mixEffect.TransitionSettings.Wipe.Rate;
        _pattern = mixEffect.TransitionSettings.Wipe.Pattern;
        _borderWidth = mixEffect.TransitionSettings.Wipe.BorderWidth;
        _borderInput = mixEffect.TransitionSettings.Wipe.BorderInput;
        _symmetry = mixEffect.TransitionSettings.Wipe.Symmetry;
        _borderSoftness = mixEffect.TransitionSettings.Wipe.BorderSoftness;
        _xPosition = mixEffect.TransitionSettings.Wipe.XPosition;
        _yPosition = mixEffect.TransitionSettings.Wipe.YPosition;
        _reverseDirection = mixEffect.TransitionSettings.Wipe.ReverseDirection;
        _flipFlop = mixEffect.TransitionSettings.Wipe.FlipFlop;
    }

    /// <summary>
    /// Rate of the wipe transition in frames
    /// </summary>
    public int Rate
    {
        get => _rate;
        set
        {
            _rate = value;
            Flag |= 1 << 0;  // Automatic flag setting for rate
        }
    }

    /// <summary>
    /// Pattern for the wipe transition
    /// </summary>
    public int Pattern
    {
        get => _pattern;
        set
        {
            _pattern = value;
            Flag |= 1 << 1;  // Automatic flag setting for pattern
        }
    }

    /// <summary>
    /// Width of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderWidth
    {
        get => _borderWidth;
        set
        {
            _borderWidth = value;
            Flag |= 1 << 2;  // Automatic flag setting for borderWidth
        }
    }

    /// <summary>
    /// Input source for the wipe border
    /// </summary>
    public int BorderInput
    {
        get => _borderInput;
        set
        {
            _borderInput = value;
            Flag |= 1 << 3;  // Automatic flag setting for borderInput
        }
    }

    /// <summary>
    /// Symmetry setting for the wipe transition as percentage (0-100%)
    /// </summary>
    public double Symmetry
    {
        get => _symmetry;
        set
        {
            _symmetry = value;
            Flag |= 1 << 4;  // Automatic flag setting for symmetry
        }
    }

    /// <summary>
    /// Softness of the wipe border as percentage (0-100%)
    /// </summary>
    public double BorderSoftness
    {
        get => _borderSoftness;
        set
        {
            _borderSoftness = value;
            Flag |= 1 << 5;  // Automatic flag setting for borderSoftness
        }
    }

    /// <summary>
    /// X position for the wipe transition (0.0-1.0)
    /// </summary>
    public double XPosition
    {
        get => _xPosition;
        set
        {
            _xPosition = value;
            Flag |= 1 << 6;  // Automatic flag setting for xPosition
        }
    }

    /// <summary>
    /// Y position for the wipe transition (0.0-1.0)
    /// </summary>
    public double YPosition
    {
        get => _yPosition;
        set
        {
            _yPosition = value;
            Flag |= 1 << 7;  // Automatic flag setting for yPosition
        }
    }

    /// <summary>
    /// Whether the wipe direction is reversed
    /// </summary>
    public bool ReverseDirection
    {
        get => _reverseDirection;
        set
        {
            _reverseDirection = value;
            Flag |= 1 << 8;  // Automatic flag setting for reverseDirection
        }
    }

    /// <summary>
    /// Whether flip flop mode is enabled
    /// </summary>
    public bool FlipFlop
    {
        get => _flipFlop;
        set
        {
            _flipFlop = value;
            Flag |= 1 << 9;  // Automatic flag setting for flipFlop
        }
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(20);
        using var writer = new BinaryWriter(memoryStream);

        writer.WriteUInt16BigEndian(Flag);  // Flag as 16-bit big endian (matching TypeScript buffer.writeUInt16BE)
        writer.Write((byte)MixEffectId);            // Mix effect index
        writer.Write((byte)Rate);                   // Rate value
        writer.Write((byte)Pattern);                // Pattern value
        writer.Pad(1);                              // 1 byte padding
        writer.WriteUInt16BigEndian((ushort)Math.Round(BorderWidth * 100));    // Border width as percentage * 100
        writer.WriteUInt16BigEndian((ushort)BorderInput);                    // Border input as 16-bit big endian
        writer.WriteUInt16BigEndian((ushort)Math.Round(Symmetry * 100));     // Symmetry as percentage * 100
        writer.WriteUInt16BigEndian((ushort)Math.Round(BorderSoftness * 100)); // Border softness as percentage * 100
        writer.WriteUInt16BigEndian((ushort)Math.Round(XPosition * 10000));  // X position as 0-1 * 10000
        writer.WriteUInt16BigEndian((ushort)Math.Round(YPosition * 10000));  // Y position as 0-1 * 10000
        writer.Write((byte)(ReverseDirection ? 1 : 0));      // Reverse direction as byte
        writer.Write((byte)(FlipFlop ? 1 : 0));              // Flip flop as byte

        return memoryStream.ToArray();
    }
}