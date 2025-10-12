using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command to set the video mode of the ATEM device
/// </summary>
[Command("CVdM")]
public class VideoModeCommand : SerializedCommand
{
    private VideoMode _mode;

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="currentState">Current ATEM state</param>
    public VideoModeCommand(AtemState currentState)
    {
        // Initialize from current state (direct field access = no flags set)
        _mode = currentState.Settings.VideoMode;
    }

    /// <summary>
    /// Video mode to set
    /// </summary>
    public VideoMode Mode
    {
        get => _mode;
        set => _mode = value; // No flag setting - this is a BasicWritableCommand equivalent
    }

    /// <summary>
    /// Serialize command to binary stream for transmission to ATEM
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);
        
        writer.Write((byte)Mode);
        writer.Pad(3); // Pad to 4 bytes total
        
        return memoryStream.ToArray();
    }
}