using AtemSharp.Enums;
using AtemSharp.Lib;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command received from ATEM device containing MultiViewer window source update
/// </summary>
[Command("MvIn")]
public class MultiViewerSourceUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// MultiViewer ID for this update
    /// </summary>
    public int MultiViewerId { get; init; }

    /// <summary>
    /// The window index within the MultiViewer
    /// </summary>
    public int WindowIndex { get; init; }

    /// <summary>
    /// The video source assigned to this window
    /// </summary>
    public int Source { get; init; }

    /// <summary>
    /// Whether this window supports VU meter display
    /// </summary>
    public bool SupportsVuMeter { get; init; }

    /// <summary>
    /// Whether this window supports safe area overlay
    /// </summary>
    public bool SupportsSafeArea { get; init; }

    /// <summary>
    /// Deserialize the command from binary stream
    /// </summary>
    public static MultiViewerSourceUpdateCommand Deserialize(ReadOnlySpan<byte> rawCommand, ProtocolVersion protocolVersion)
    {
        return new MultiViewerSourceUpdateCommand
        {
            MultiViewerId = rawCommand.ReadUInt8(0),
            WindowIndex = rawCommand.ReadUInt8(1),
            Source = rawCommand.ReadUInt16BigEndian(2),
            SupportsVuMeter = rawCommand.ReadBoolean(4),
            SupportsSafeArea = rawCommand.ReadBoolean(5)
        };
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
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
            SafeTitle = currentWindow.SafeTitle,
            AudioMeter = currentWindow.AudioMeter
        };

        // Update the window in the MultiViewer
        multiViewer.Windows[WindowIndex] = updatedWindow;
    }
}
