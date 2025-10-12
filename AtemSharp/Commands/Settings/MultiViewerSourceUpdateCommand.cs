using AtemSharp.Commands;
using AtemSharp.Enums;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings;

/// <summary>
/// Command received from ATEM device containing MultiViewer window source update
/// </summary>
[Command("MvIn")]
public class MultiViewerSourceUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// MultiViewer ID for this update
    /// </summary>
    public int MultiViewerId { get; set; }

    /// <summary>
    /// The window index within the MultiViewer
    /// </summary>
    public int WindowIndex { get; set; }

    /// <summary>
    /// The video source assigned to this window
    /// </summary>
    public int Source { get; set; }

    /// <summary>
    /// Whether this window supports VU meter display
    /// </summary>
    public bool SupportsVuMeter { get; set; }

    /// <summary>
    /// Whether this window supports safe area overlay
    /// </summary>
    public bool SupportsSafeArea { get; set; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    /// <param name="stream">Binary stream containing command data</param>
    /// <param name="protocolVersion">Protocol version used for deserialization</param>
    /// <returns>Deserialized command instance</returns>
    public static MultiViewerSourceUpdateCommand Deserialize(Stream stream, ProtocolVersion protocolVersion)
    {
        using var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true);

        var multiViewerId = reader.ReadByte();
        var windowIndex = reader.ReadByte();
        var source = reader.ReadUInt16BigEndian();
        var supportsVuMeter = reader.ReadByte() != 0;
        var supportsSafeArea = reader.ReadByte() != 0;

        return new MultiViewerSourceUpdateCommand
        {
            MultiViewerId = multiViewerId,
            WindowIndex = windowIndex,
            Source = source,
            SupportsVuMeter = supportsVuMeter,
            SupportsSafeArea = supportsSafeArea
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
        }

        // Create updated window state with new properties
        var updatedWindow = new MultiViewerWindowState
        {
            WindowIndex = WindowIndex,
            Source = Source,
            SupportsVuMeter = SupportsVuMeter,
            SupportsSafeArea = SupportsSafeArea,
            // Preserve existing optional properties if they exist
            SafeTitle = currentWindow?.SafeTitle,
            AudioMeter = currentWindow?.AudioMeter
        };

        // Update the window in the MultiViewer
        multiViewer.Windows[WindowIndex] = updatedWindow;

        // Return the state path that was modified for change tracking
        return [$"settings.multiViewers.{MultiViewerId}.windows.{WindowIndex}"];
    }
}