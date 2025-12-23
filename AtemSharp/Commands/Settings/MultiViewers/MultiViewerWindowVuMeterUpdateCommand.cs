using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

[Command("VuMC")]
internal partial class MultiViewerWindowVuMeterUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _multiViewerId;
    [DeserializedField(1)] private byte _windowIndex;
    [DeserializedField(2)] private bool _vuEnabled;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        // Get the MultiViewer and update its window
        var multiViewer = state.Settings.MultiViewers[MultiViewerId];

        var currentWindow = multiViewer.Windows.GetOrCreate(WindowIndex);
        currentWindow.AudioMeter = VuEnabled;
    }
}
