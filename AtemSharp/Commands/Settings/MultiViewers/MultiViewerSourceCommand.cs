using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command to set the video source for a specific MultiViewer window
/// </summary>
[Command("CMvI")]
public class MultiViewerSourceCommand : SerializedCommand
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
    /// The video source number to assign to this window
    /// </summary>
    public int Source { get; set; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if MultiViewer ID is invalid</exception>
    public MultiViewerSourceCommand(int multiViewerId, AtemState currentState)
    {
        MultiViewerId = multiViewerId;

        // Validate MultiViewer exists
        if (currentState.Info.MultiViewer.Count == 0 || multiViewerId >= currentState.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", multiViewerId);
        }

        // Initialize with defaults
        WindowIndex = 0;
        Source = 0;
    }

    /// <summary>
    /// Convenience constructor for setting window and source directly
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="windowIndex">Window index within the MultiViewer</param>
    /// <param name="source">Video source number</param>
    /// <param name="currentState">Current ATEM state</param>
    public MultiViewerSourceCommand(int multiViewerId, int windowIndex, int source, AtemState currentState) 
        : this(multiViewerId, currentState)
    {
        WindowIndex = windowIndex;
        Source = source;
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
        
        // Write source as big-endian UInt16
        writer.WriteUInt16BigEndian((ushort)Source);
        
        return memoryStream.ToArray();
    }
}