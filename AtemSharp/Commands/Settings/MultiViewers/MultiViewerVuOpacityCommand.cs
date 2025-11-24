using AtemSharp.State;

namespace AtemSharp.Commands.Settings.MultiViewers;

// TODO: Split into two commands to be consistent with other commands
/// <summary>
/// Command to set MultiViewer VU opacity level
/// </summary>
[Command("VuMo")]
[BufferSize(4)]
public partial class MultiViewerVuOpacityCommand(MultiViewer multiViewer) : SerializedCommand
{
    [SerializedField(0)]
    [NoProperty]
    internal readonly byte MultiViewerId = multiViewer.Index;

    /// <summary>
    /// VU opacity level (0-100)
    /// </summary>
    [SerializedField(1)]
    private byte _opacity = multiViewer.VuOpacity;
}
