using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command to enable or disable safe area display for a specific MultiViewer window
/// </summary>
[Command("SaMw")]
public partial class MultiViewerWindowSafeAreaUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _multiViewerId;

    /// <summary>
    /// The window index within the MultiViewer to update
    /// </summary>
    [DeserializedField(1)] private byte _windowIndex;

    /// <summary>
    /// Whether safe area display should be enabled for this window
    /// </summary>
    [DeserializedField(2)] private bool _safeAreaEnabled;

    /// <summary>
    /// Parameterless constructor for deserialization
    /// </summary>
    public MultiViewerWindowSafeAreaUpdateCommand()
    {
    }

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var multiViewer = state.Settings.MultiViewers[MultiViewerId];

        // Get the current window state or create a new one
        if (!multiViewer.Windows.TryGetValue(WindowIndex, out var currentWindow))
        {
            currentWindow = new MultiViewerWindowState();
            multiViewer.Windows[WindowIndex] = currentWindow;
        }

        // Update the safe area state
        currentWindow.SafeTitle = SafeAreaEnabled;
    }
}
