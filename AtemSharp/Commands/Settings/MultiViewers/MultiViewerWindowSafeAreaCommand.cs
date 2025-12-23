using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Used to enable or disable safe area display for a specific MultiViewer window
/// </summary>
[Command("SaMw")]
[BufferSize(4)]
public partial class MultiViewerWindowSafeAreaCommand(MultiViewerWindowState window) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] internal readonly byte MultiViewerId = window.MultiViewerId;

    /// <summary>
    /// The window index within the MultiViewer to update
    /// </summary>
    [SerializedField(1)] private byte _windowIndex = window.WindowIndex;

    /// <summary>
    /// Whether safe area display should be enabled for this window
    /// </summary>
    [SerializedField(2)] private bool _safeAreaEnabled = window.SafeTitle;
}
