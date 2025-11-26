using AtemSharp.State.Settings.MultiViewer;

namespace AtemSharp.Commands.Settings.MultiViewers;

/// <summary>
/// Command to set MultiViewer VU opacity level
/// </summary>
[Command("VuMo")]
[BufferSize(4)]
public partial class MultiViewerVuOpacityCommand(MultiViewer multiViewer) : SerializedCommand
{
    [SerializedField(0)] [NoProperty] private readonly byte _multiViewerId = multiViewer.Id;

    /// <summary>
    /// VU opacity level (0-100)
    /// </summary>
    [SerializedField(1)] private byte _opacity = multiViewer.VuOpacity;
}
