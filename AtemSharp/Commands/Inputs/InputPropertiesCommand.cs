using AtemSharp.Enums;
using AtemSharp.Enums.Ports;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Inputs;

/// <summary>
/// Command to update input channel properties
/// </summary>
[Command("CInL")]
public class InputPropertiesCommand : SerializedCommand
{
    private string _longName = string.Empty;
    private string _shortName = string.Empty;
    private ExternalPortType _externalPortType;

    /// <summary>
    /// Input identifier/number
    /// </summary>
    public int InputId { get; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="inputId">Input identifier/number</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if input not available</exception>
    public InputPropertiesCommand(int inputId, AtemState currentState)
    {
        InputId = inputId;

        // If no video state or input exists, initialize with defaults
        if (!currentState.Video.Inputs.TryGetValue(inputId, out var inputChannel))
        {
            // Set default values and flags (like TypeScript pattern)
            LongName = string.Empty;
            ShortName = string.Empty;
            ExternalPortType = ExternalPortType.Unknown;
            return;
        }

        // Initialize from current state (direct field access = no flags set)
        _longName = inputChannel.LongName;
        _shortName = inputChannel.ShortName;
        _externalPortType = inputChannel.ExternalPortType;
    }

    /// <summary>
    /// Long descriptive name for the input (max 20 characters)
    /// </summary>
    public string LongName
    {
        get => _longName;
        set
        {
            _longName = value ?? string.Empty;
            Flag |= 1 << 0;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// Short name for the input (max 4 characters)
    /// </summary>
    public string ShortName
    {
        get => _shortName;
        set
        {
            _shortName = value ?? string.Empty;
            Flag |= 1 << 1;  // Automatic flag setting!
        }
    }

    /// <summary>
    /// External port type for the input
    /// </summary>
    public ExternalPortType ExternalPortType
    {
        get => _externalPortType;
        set
        {
            _externalPortType = value;
            Flag |= 1 << 2;  // Automatic flag setting!
        }
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(32);
        using var writer = new BinaryWriter(memoryStream);
        
        // Write flag as single byte
        writer.Write((byte)Flag);
        writer.Pad(1); // Padding byte to match TypeScript structure
        
        // Write input ID at position 2-3
        writer.WriteUInt16BigEndian((ushort)InputId);
        
        // Write long name (20 bytes, null-terminated) at position 4-23
        var longNameBytes = new byte[20];
        AtemUtil.WriteNullTerminatedString(LongName, longNameBytes, 0, 20);
        writer.Write(longNameBytes);
        
        // Write short name (4 bytes, null-terminated) at position 24-27 
        var shortNameBytes = new byte[4];
        AtemUtil.WriteNullTerminatedString(ShortName, shortNameBytes, 0, 4);
        writer.Write(shortNameBytes);
        
        // Write external port type at position 28-29
        writer.WriteUInt16BigEndian((ushort)ExternalPortType);
        
        // Pad to ensure exactly 32 bytes total (positions 30-31)
        writer.Pad(2);
        
        return memoryStream.ToArray();
    }
}