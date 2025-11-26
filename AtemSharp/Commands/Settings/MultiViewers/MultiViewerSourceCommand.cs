using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command to set the video source for a specific MultiViewer window
/// </summary>
[Command("CMvI")]
[BufferSize(4)]
public partial class MultiViewerSourceCommand(MultiViewerWindowState window) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] internal readonly byte MultiViewerId = window.MultiViewerId;

    [SerializedField(1)] [NoProperty] internal readonly byte WindowIndex = window.WindowIndex;

    /// <summary>
    /// The video source number to assign to this window
    /// </summary>
    [SerializedField(2)] private ushort _source = window.Source;
}
