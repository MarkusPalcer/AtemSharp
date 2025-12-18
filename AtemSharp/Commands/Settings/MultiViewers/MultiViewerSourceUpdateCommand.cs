using AtemSharp.State;

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
        var window = multiViewer.Windows.GetOrCreate(WindowIndex);

        window.Source = Source;
        window.SupportsVuMeter = SupportsVuMeter;
        window.SupportsSafeArea = SupportsSafeArea;
    }
}
