using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

[Command("MvIn")]
internal partial class MultiViewerSourceUpdateCommand : IDeserializedCommand
{
    [DeserializedField(0)] private byte _multiViewerId;
    [DeserializedField(1)] private byte _windowIndex;
    [DeserializedField(2)] private ushort _source;
    [DeserializedField(4)] private bool _supportsVuMeter;
    [DeserializedField(5)] private bool _supportsSafeArea;

    /// <inheritdoc />
    public void ApplyToState(AtemState state)
    {
        var multiViewer = state.Settings.MultiViewers[MultiViewerId];
        var window = multiViewer.Windows.GetOrCreate(WindowIndex);

        window.Source = Source;
        window.SupportsVuMeter = SupportsVuMeter;
        window.SupportsSafeArea = SupportsSafeArea;
    }
}
