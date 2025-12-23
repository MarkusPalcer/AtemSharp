using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Used to set the video source for a specific MultiViewer window
/// </summary>
[Command("CMvI")]
[BufferSize(4)]
public partial class MultiViewerSourceCommand(MultiViewerWindowState window) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] internal readonly byte MultiViewerId = window.MultiViewerId;

    [SerializedField(1)] [NoProperty] internal readonly byte WindowIndex = window.WindowIndex;

    [SerializedField(2)] private ushort _source = window.Source;
}
