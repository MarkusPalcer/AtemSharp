using AtemSharp.State;
using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command received from ATEM device containing MultiViewer window source update
/// </summary>
[Command("MvIn")]
public partial class MultiViewerSourceUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// MultiViewer ID for this update
    /// </summary>
    [DeserializedField(0)] private byte _multiViewerId;

    /// <summary>
    /// The window index within the MultiViewer
    /// </summary>
    [DeserializedField(1)] private byte _windowIndex;

    /// <summary>
    /// The video source assigned to this window
    /// </summary>
    [DeserializedField(2)] private ushort _source;

    /// <summary>
    /// Whether this window supports VU meter display
    /// </summary>
    [DeserializedField(4)] private bool _supportsVuMeter;

    /// <summary>
    /// Whether this window supports safe area overlay
    /// </summary>
    [DeserializedField(5)] private bool _supportsSafeArea;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var multiViewer = state.Settings.MultiViewers[MultiViewerId];

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
