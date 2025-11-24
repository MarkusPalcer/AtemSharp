using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command to enable or disable VU meter display for a specific MultiViewer window
/// </summary>
[Command("VuMS")]
[BufferSize(4)]
public partial class MultiViewerWindowVuMeterCommand(MultiViewerWindowState window) : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    internal readonly byte MultiViewerId = window.MultiViewerId;

    [SerializedField(1)]
    [NoProperty]
    internal readonly byte WindowIndex = window.WindowIndex;

    [SerializedField(2)]
    private bool _vuEnabled = window.AudioMeter;
}
