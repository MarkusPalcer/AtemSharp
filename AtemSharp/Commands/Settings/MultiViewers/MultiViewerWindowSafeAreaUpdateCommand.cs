using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

[Command("SaMw")]
internal partial class MultiViewerWindowSafeAreaUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _multiViewerId;
    [DeserializedField(1)] private byte _windowIndex;
    [DeserializedField(2)] private bool _safeAreaEnabled;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var multiViewer = state.Settings.MultiViewers[MultiViewerId];

        // Get the current window state or create a new one
        var currentWindow = multiViewer.Windows.GetOrCreate(WindowIndex);
        currentWindow.SafeTitle = SafeAreaEnabled;
    }
}
