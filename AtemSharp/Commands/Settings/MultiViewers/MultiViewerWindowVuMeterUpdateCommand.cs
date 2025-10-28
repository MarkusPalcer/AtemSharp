using AtemSharp.Helpers;
using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command received from ATEM device containing MultiViewer window VU meter update
/// </summary>
[Command("VuMC")]
public partial class MultiViewerWindowVuMeterUpdateCommand : IDeserializedCommand
{
    /// <summary>
    /// MultiViewer ID for this update
    /// </summary>
    [DeserializedField(0)]
    private byte _multiViewerId;

    /// <summary>
    /// The window index within the MultiViewer
    /// </summary>
    [DeserializedField(1)]
    private byte _windowIndex;

    /// <summary>
    /// Whether VU meter display is enabled for this window
    /// </summary>
    [DeserializedField(2)]
    private bool _vuEnabled;

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
            multiViewer.Windows[WindowIndex] = currentWindow;
        }

        // Update the VU meter state
        currentWindow.AudioMeter = VuEnabled;
    }
}
