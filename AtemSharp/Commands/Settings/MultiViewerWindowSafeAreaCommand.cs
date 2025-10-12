using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command to enable or disable safe area display for a specific MultiViewer window
/// </summary>
[Command("SaMw")]
public class MultiViewerWindowSafeAreaCommand : SerializedCommand, IDeserializedCommand
{
    /// <summary>
    /// MultiViewer ID for this command
    /// </summary>
    public int MultiViewerId { get; set; }

    /// <summary>
    /// The window index within the MultiViewer to update
    /// </summary>
    public int WindowIndex { get; set; }

    /// <summary>
    /// Whether safe area display should be enabled for this window
    /// </summary>
    public bool SafeAreaEnabled { get; set; }

    /// <summary>
    /// Create command initialized with current state values
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="currentState">Current ATEM state</param>
    /// <exception cref="InvalidIdError">Thrown if MultiViewer ID is invalid</exception>
    public MultiViewerWindowSafeAreaCommand(int multiViewerId, AtemState currentState)
    {
        MultiViewerId = multiViewerId;

        // Validate MultiViewer exists
        if (currentState.Info.MultiViewer.Count == 0 || multiViewerId >= currentState.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", multiViewerId);
        }

        // Initialize with defaults
        WindowIndex = 0;
        SafeAreaEnabled = false;
    }

    /// <summary>
    /// Convenience constructor for setting window and safe area state directly
    /// </summary>
    /// <param name="multiViewerId">MultiViewer ID (0-based)</param>
    /// <param name="windowIndex">Window index within the MultiViewer</param>
    /// <param name="safeAreaEnabled">Whether safe area should be enabled</param>
    /// <param name="currentState">Current ATEM state</param>
    public MultiViewerWindowSafeAreaCommand(int multiViewerId, int windowIndex, bool safeAreaEnabled, AtemState currentState)
        : this(multiViewerId, currentState)
    {
        WindowIndex = windowIndex;
        SafeAreaEnabled = safeAreaEnabled;
    }

    /// <summary>
    /// Parameterless constructor for deserialization
    /// </summary>
    public MultiViewerWindowSafeAreaCommand()
    {
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
        
        // Write safe area enabled flag
        writer.WriteBoolean(SafeAreaEnabled);
        
        // Pad to 4 bytes total
        writer.Pad(1);
        
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MultiViewerWindowSafeAreaCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var multiViewerId = reader.ReadByte();
        var windowIndex = reader.ReadByte();
        var safeAreaEnabled = reader.ReadBoolean();

        return new MultiViewerWindowSafeAreaCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            SafeAreaEnabled = safeAreaEnabled
        };
    }

    /// <inheritdoc />
    public string[] ApplyToState(AtemState state)
    {
        // Validate state prerequisites (same pattern as TypeScript update commands)
        if (state.Info.MultiViewer.Count == 0 || MultiViewerId >= state.Info.MultiViewer.Count)
        {
            throw new InvalidIdError("MultiViewer", MultiViewerId);
        }

        // Get the MultiViewer and update its window
        var multiViewer = AtemStateUtil.GetMultiViewer(state, MultiViewerId);
        
        // Get the current window state or create a new one
        if (!multiViewer.Windows.TryGetValue(WindowIndex, out var currentWindow))
        {
            currentWindow = new MultiViewerWindowState();
            multiViewer.Windows[WindowIndex] = currentWindow;
        }

        // Update the safe area state
        currentWindow.SafeTitle = SafeAreaEnabled;

        // Return the state path that was modified for change tracking
        return new[] { $"settings.multiViewers.{MultiViewerId}.windows.{WindowIndex}.safeTitle" };
    }
}