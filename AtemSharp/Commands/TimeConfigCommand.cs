using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands;

/// <summary>
/// Command to set the time configuration mode for the ATEM device
/// </summary>
[Command("CTCC", ProtocolVersion.V8_1_1)]
public class TimeConfigCommand : SerializedCommand
{
    private TimeMode _mode;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    public TimeConfigCommand(AtemState currentState)
    {
        // Initialize from current state (direct field access = no flags set)
        _mode = currentState.Settings.TimeMode;
    }

    /// <summary>
    /// Time mode for the ATEM device
    /// </summary>
    public TimeMode Mode
    {
        get => _mode;
        set => _mode = value; // No flag setting - this is a BasicWritableCommand equivalent
    }

    /// <inheritdoc />
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);
        
        // Write the mode as a single byte
        writer.Write((byte)Mode);
        writer.Pad(3); // Pad to 4-byte total length to match TypeScript buffer size
        
        return memoryStream.ToArray();
    }
}