using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command to enable or disable VU meter display for a specific MultiViewer window
/// </summary>
[Command("VuMS")]
public class MultiViewerWindowVuMeterCommand : SerializedCommand
{
    /// <summary>
    /// MultiViewer ID for this command
    /// </summary>
    public int MultiViewerId { get; }

    /// <summary>
    /// The window index within the MultiViewer to update
    /// </summary>
    public int WindowIndex { get; set; }

    /// <summary>
    /// Whether VU meter display should be enabled for this window
    /// </summary>
    public bool VuEnabled { get; set; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if MultiViewer ID is invalid</exception>
    public MultiViewerWindowVuMeterCommand(int multiViewerId, AtemState currentState)
    {
        MultiViewerId = multiViewerId;

        // Validate MultiViewer exists
        if (currentState.Info.MultiViewer.Count == 0 || multiViewerId >= currentState.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", multiViewerId);
        }

        // Initialize with defaults
        WindowIndex = 0;
        VuEnabled = false;
    }

    /// <summary>
    /// Convenience constructor for setting window and VU meter state directly
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="windowIndex">Window index within the MultiViewer</param>
    /// <param name="vuEnabled">Whether VU meter should be enabled</param>
    /// <param name="currentState">Current ATEM state</param>
    public MultiViewerWindowVuMeterCommand(int multiViewerId, int windowIndex, bool vuEnabled, AtemState currentState)
        : this(multiViewerId, currentState)
    {
        WindowIndex = windowIndex;
        VuEnabled = vuEnabled;
    }

    /// <summary>
    /// Serialize command to binary stream
    /// </summary>
    /// <param name="version">Protocol version</param>
    /// <returns>Serialized command data</returns>
    public override byte[] Serialize(ProtocolVersion version)
    {
        using var memoryStream = new MemoryStream(4);
        using var writer = new BinaryWriter(memoryStream);
        
        // Write MultiViewer ID
        writer.Write((byte)MultiViewerId);
        
        // Write window index
        writer.Write((byte)WindowIndex);
        
        // Write VU meter enabled flag
        writer.WriteBoolean(VuEnabled);
        
        // Pad to 4 bytes total
        writer.Pad(1);
        
        return memoryStream.ToArray();
    }
}